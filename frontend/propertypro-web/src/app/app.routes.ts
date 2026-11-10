import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { adminGuard } from './core/auth/admin.guard';

export const routes: Routes = [
  {
    path: 'app',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./layout/shell/app-shell.component').then(
        (component) => component.AppShellComponent,
      ),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'facilities' },
      {
        path: 'users',
        canActivate: [adminGuard],
        loadComponent: () =>
          import('./features/users/pages/user-management/user-management.component').then(
            (component) => component.UserManagementComponent,
          ),
      },
      {
        path: 'facilities/new',
        loadComponent: () =>
          import('./features/facilities/pages/facility-form/facility-form.component').then(
            (component) => component.FacilityFormComponent,
          ),
      },
      {
        path: 'facilities/:id/edit',
        loadComponent: () =>
          import('./features/facilities/pages/facility-form/facility-form.component').then(
            (component) => component.FacilityFormComponent,
          ),
      },
      {
        path: 'facilities/:id',
        loadComponent: () =>
          import('./features/facilities/pages/facility-detail/facility-detail.component').then(
            (component) => component.FacilityDetailComponent,
          ),
      },
      {
        path: 'facilities',
        loadComponent: () =>
          import('./features/facilities/pages/facility-list/facility-list.component').then(
            (component) => component.FacilityListComponent,
          ),
      },
      {
        path: 'buildings/new',
        loadComponent: () => import('./features/buildings/pages/building-form/building-form.component').then(component => component.BuildingFormComponent),
      },
      {
        path: 'buildings/:id/edit',
        loadComponent: () => import('./features/buildings/pages/building-form/building-form.component').then(component => component.BuildingFormComponent),
      },
      {
        path: 'buildings/:id',
        loadComponent: () => import('./features/buildings/pages/building-detail/building-detail.component').then(component => component.BuildingDetailComponent),
      },
      {
        path: 'buildings',
        loadComponent: () =>
          import('./features/buildings/pages/building-list/building-list.component').then(
            (component) => component.BuildingListComponent,
          ),
      },
      { path: 'assets/new', loadComponent: () => import('./features/assets/pages/asset-form/asset-form.component').then(component => component.AssetFormComponent) },
      { path: 'assets/:id/edit', loadComponent: () => import('./features/assets/pages/asset-form/asset-form.component').then(component => component.AssetFormComponent) },
      { path: 'assets/:id', loadComponent: () => import('./features/assets/pages/asset-detail/asset-detail.component').then(component => component.AssetDetailComponent) },
      {
        path: 'assets',
        loadComponent: () =>
          import('./features/assets/pages/asset-list/asset-list.component').then(
            (component) => component.AssetListComponent,
          ),
      },
    ],
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login.component').then(
        (component) => component.LoginComponent,
      ),
  },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: '**', redirectTo: 'login' },
];
