import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { AuthSessionService } from '../../../../core/auth/auth-session.service';
import { ManagedUser, OrganizationOption, UsersApiService } from '../../data-access/users-api.service';

@Component({ selector:'app-user-management', imports:[FormsModule,ButtonModule,InputTextModule,SelectModule,TableModule,TagModule], templateUrl:'./user-management.component.html', styleUrl:'./user-management.component.scss' })
export class UserManagementComponent {
  private readonly api=inject(UsersApiService);private readonly session=inject(AuthSessionService);
  readonly users=signal<ManagedUser[]>([]);readonly organizations=signal<OrganizationOption[]>([]);readonly selectedRoles=signal<Record<string,string>>({});readonly selectedOrganizations=signal<Record<string,number|null>>({});readonly search=signal('');readonly loading=signal(true);readonly busyUser=signal<string|null>(null);readonly error=signal<string|null>(null);readonly notice=signal<string|null>(null);
  readonly availableRoles=['Admin','FacilityManager','MaintenanceManager','Technician','Viewer'];
  constructor(){this.load()}
  load():void{this.loading.set(true);this.error.set(null);this.api.getUsers().subscribe({next:users=>{this.users.set(users);this.loading.set(false)},error:e=>{this.fail(e,'Users could not be loaded.');this.loading.set(false)}});this.api.getOrganizations().subscribe({next:x=>this.organizations.set(x),error:e=>this.fail(e,'Organizations could not be loaded.')})}
  matches(user:ManagedUser):boolean{const term=this.search().trim().toLowerCase();return !term||`${user.firstName} ${user.lastName} ${user.email}`.toLowerCase().includes(term)}
  visibleUsers():ManagedUser[]{return this.users().filter(user=>this.matches(user))}
  roleOption(user:ManagedUser):string{return this.selectedRoles()[user.id]??this.availableRoles.find(role=>!user.roles.some(value=>value.toLowerCase()===role.toLowerCase()))??this.availableRoles[0]}
  organizationName(id:number|null):string{return this.organizations().find(x=>x.id===id)?.name??(id?'Unknown organization':'Unassigned')}
  setRole(userId:string,role:string):void{this.selectedRoles.update(values=>({...values,[userId]:role}))}
  setOrganization(userId:string,id:number|null):void{this.selectedOrganizations.update(values=>({...values,[userId]:id}))}
  assignRole(user:ManagedUser):void{const role=this.selectedRoles()[user.id];if(!role||user.roles.some(x=>x.toLowerCase()===role.toLowerCase()))return;this.busyUser.set(user.id);this.notice.set(null);this.api.assignRole(user.id,role).subscribe({next:()=>{this.users.update(users=>users.map(x=>x.id===user.id?{...x,roles:[...x.roles,role]}:x));this.busyUser.set(null);this.notice.set(`${role} assigned to ${user.email}.`)},error:e=>{this.busyUser.set(null);this.fail(e,'Role could not be assigned.')}})}
  removeRole(user:ManagedUser,role:string):void{if(!window.confirm(`Remove ${role} from ${user.email}?`))return;this.busyUser.set(user.id);this.notice.set(null);this.api.removeRole(user.id,role).subscribe({next:()=>{this.users.update(users=>users.map(x=>x.id===user.id?{...x,roles:x.roles.filter(value=>value!==role)}:x));this.busyUser.set(null);this.notice.set(`${role} removed from ${user.email}.`)},error:e=>{this.busyUser.set(null);this.fail(e,'Role could not be removed.')}})}
  assignOrganization(user:ManagedUser):void{const id=this.selectedOrganizations()[user.id];if(!id||id===user.organizationId)return;this.busyUser.set(user.id);this.notice.set(null);this.api.assignOrganization(user.id,id).subscribe({next:()=>{this.users.update(users=>users.map(x=>x.id===user.id?{...x,organizationId:id}:x));this.busyUser.set(null);this.notice.set(`Organization updated for ${user.email}. They must sign in again to receive the updated access.`)},error:e=>{this.busyUser.set(null);this.fail(e,'Organization could not be assigned.')}})}
  isCurrentUser(user:ManagedUser):boolean{return this.session.currentUser()?.userId===user.id}
  private fail(error:HttpErrorResponse,fallback:string):void{this.error.set(error.status===0?'The API could not be reached.':error.error?.detail??fallback)}
}
