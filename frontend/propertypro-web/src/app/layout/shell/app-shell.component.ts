import { Component, computed, DestroyRef, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { AuthSessionService } from '../../core/auth/auth-session.service';

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, AvatarModule, ButtonModule],
  templateUrl: './app-shell.component.html',
  styleUrl: './app-shell.component.scss',
})
export class AppShellComponent {
  private readonly session = inject(AuthSessionService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);

  readonly isSidebarOpen = signal(false);
  readonly currentSection = signal(this.sectionFor(this.router.url));
  readonly user = computed(() => this.session.currentUser());
  readonly userName = computed(() => {
    const user = this.user();
    return user ? `${user.firstName} ${user.lastName}`.trim() : 'PropertyPro user';
  });
  readonly initials = computed(() =>
    this.userName()
      .split(/\s+/)
      .filter(Boolean)
      .slice(0, 2)
      .map((part) => part[0].toUpperCase())
      .join(''),
  );

  constructor() {
    this.router.events.pipe(filter((event) => event instanceof NavigationEnd), takeUntilDestroyed(this.destroyRef)).subscribe((event) => {
      if (event instanceof NavigationEnd) this.currentSection.set(this.sectionFor(event.urlAfterRedirects));
    });
  }

  private sectionFor(url: string): string {
    const path = url.split('?')[0].split('/')[2];
    if (path === 'buildings') return 'Buildings';
    if (path === 'assets') return 'Assets';
    return 'Facilities';
  }

  closeSidebar(): void {
    this.isSidebarOpen.set(false);
  }

  logout(): void {
    this.session.clear();
    void this.router.navigateByUrl('/login');
  }
}
