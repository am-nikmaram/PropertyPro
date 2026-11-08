import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { BuildingsApiService, Building, BuildingWriteRequest, InventoryFacility } from '../../buildings-api.service';

@Component({ selector:'app-building-form', imports:[FormsModule,RouterLink,ButtonModule,InputNumberModule,InputTextModule,TextareaModule], templateUrl:'./building-form.component.html', styleUrl:'./building-form.component.scss' })
export class BuildingFormComponent {
  private readonly api=inject(BuildingsApiService); private readonly route=inject(ActivatedRoute); private readonly router=inject(Router);
  readonly id=Number(this.route.snapshot.paramMap.get('id'))||null; readonly facilities=signal<InventoryFacility[]>([]); readonly loading=signal(!!this.id); readonly saving=signal(false); readonly error=signal<string|null>(null);
  form:BuildingWriteRequest={facilityId:Number(this.route.snapshot.queryParamMap.get('facilityId'))||0,name:'',code:'',description:null,yearBuilt:null,totalArea:null};
  constructor(){this.api.getFacilities().subscribe({next:items=>{this.facilities.set(items);if(!this.form.facilityId&&items.length)this.form.facilityId=items[0].id;},error:e=>this.showError(e,'Facilities could not be loaded.')});if(this.id)this.api.getBuilding(this.id).subscribe({next:item=>{this.form=this.toRequest(item);this.loading.set(false);},error:e=>{this.loading.set(false);this.showError(e,'Building could not be loaded.');}});}
  save():void{this.saving.set(true);this.error.set(null);const req={...this.form,name:this.form.name.trim(),code:this.form.code.trim(),description:this.form.description?.trim()||null};const action=this.id?this.api.updateBuilding(this.id,req):this.api.createBuilding(req);action.subscribe({next:item=>{this.saving.set(false);void this.router.navigate(['/app/buildings',item.id]);},error:e=>{this.saving.set(false);this.showError(e,'Building could not be saved.');}});}
  private toRequest(item:Building):BuildingWriteRequest{return{facilityId:item.facilityId,name:item.name,code:item.code,description:item.description,yearBuilt:item.yearBuilt,totalArea:item.totalArea};}
  private showError(e:HttpErrorResponse,fallback:string):void{this.error.set(e.status===0?'The API could not be reached.':e.error?.detail??fallback);}
}
