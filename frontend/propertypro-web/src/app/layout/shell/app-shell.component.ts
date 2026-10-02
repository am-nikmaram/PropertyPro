import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
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

  readonly isSidebarOpen = signal(false);
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

  closeSidebar(): void {
    this.isSidebarOpen.set(false);
  }

  logout(): void {
    this.session.clear();
    void this.router.navigateByUrl('/login');
  }
}
