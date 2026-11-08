import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { AssetsApiService, AssetDetail } from '../../assets-api.service';
import { AuthSessionService } from '../../../../core/auth/auth-session.service';

@Component({selector:'app-asset-detail',imports:[RouterLink,ButtonModule,TagModule,CurrencyPipe,DatePipe],templateUrl:'./asset-detail.component.html',styleUrl:'./asset-detail.component.scss'})
export class AssetDetailComponent{
 private readonly api=inject(AssetsApiService);private readonly router=inject(Router);private readonly session=inject(AuthSessionService);private readonly id=Number(inject(ActivatedRoute).snapshot.paramMap.get('id'));readonly item=signal<AssetDetail|null>(null);readonly loading=signal(true);readonly error=signal<string|null>(null);canManage():boolean{return this.session.hasAnyRole('Admin','MaintenanceManager')}
 constructor(){this.load()}load():void{this.loading.set(true);this.api.getAsset(this.id).subscribe({next:x=>{this.item.set(x);this.loading.set(false)},error:e=>{this.fail(e);this.loading.set(false)}})}deactivate():void{const x=this.item();if(!x||!window.confirm(`Deactivate ${x.name}?`))return;this.api.deleteAsset(this.id).subscribe({next:()=>void this.router.navigateByUrl('/app/assets'),error:e=>this.fail(e)})}statusName(status:number):string{return({1:'Active',2:'In maintenance',3:'Under repair',4:'Retired',5:'Disposed',6:'Lost'}as Record<number,string>)[status]??`Status ${status}`}criticalityName(value:number):string{return({1:'Low',2:'Medium',3:'High',4:'Critical'}as Record<number,string>)[value]??`Level ${value}`}private fail(e:HttpErrorResponse):void{this.error.set(e.status===0?'The API could not be reached.':e.error?.detail??'Asset could not be loaded.')}
}
