import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';

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
        path: 'facilities',
        loadComponent: () =>
          import('./features/facilities/pages/facility-list/facility-list.component').then(
            (component) => component.FacilityListComponent,
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
