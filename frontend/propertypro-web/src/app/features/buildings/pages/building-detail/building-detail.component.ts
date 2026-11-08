import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { BuildingsApiService, Building } from '../../buildings-api.service';
import { AuthSessionService } from '../../../../core/auth/auth-session.service';

@Component({selector:'app-building-detail',imports:[RouterLink,ButtonModule,TagModule],templateUrl:'./building-detail.component.html',styleUrl:'./building-detail.component.scss'})
export class BuildingDetailComponent{
 private readonly api=inject(BuildingsApiService);private readonly router=inject(Router);private readonly session=inject(AuthSessionService);private readonly id=Number(inject(ActivatedRoute).snapshot.paramMap.get('id'));readonly item=signal<Building|null>(null);readonly loading=signal(true);readonly error=signal<string|null>(null);canManage():boolean{return this.session.hasAnyRole('Admin','FacilityManager')}
 constructor(){this.load()}load():void{this.loading.set(true);this.api.getBuilding(this.id).subscribe({next:x=>{this.item.set(x);this.loading.set(false)},error:e=>{this.fail(e);this.loading.set(false)}})}deactivate():void{const x=this.item();if(!x||!window.confirm(`Deactivate ${x.name}?`))return;this.api.deleteBuilding(this.id).subscribe({next:()=>void this.router.navigateByUrl('/app/buildings'),error:e=>this.fail(e)})}private fail(e:HttpErrorResponse):void{this.error.set(e.status===0?'The API could not be reached.':e.error?.detail??'Building could not be loaded.')}
}
