import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormField, FormRoot, email, form, maxLength, required } from '@angular/forms/signals';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { AuthApiService } from '../../data-access/auth-api.service';

@Component({
  selector: 'app-login',
  imports: [FormField, FormRoot, InputTextModule, PasswordModule, ButtonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private readonly authApi = inject(AuthApiService);
  private readonly router = inject(Router);

  readonly errorMessage = signal<string | null>(null);
  readonly loginModel = signal({ email: '', password: '' });
  readonly loginForm = form(this.loginModel, (path) => {
    required(path.email, { message: 'Work email is required.' });
    email(path.email, { message: 'Enter a valid email address.' });
    maxLength(path.email, 256, { message: 'Email must be 256 characters or fewer.' });
    required(path.password, { message: 'Password is required.' });
  }, {
    submission: {
      action: async (field) => {
        this.errorMessage.set(null);
        try {
          await firstValueFrom(this.authApi.login(field().value()));
          await this.router.navigateByUrl('/app');
          return;
        } catch (error) {
          const message = this.getLoginError(error);
          this.errorMessage.set(message);
          return { kind: 'loginFailed', message };
        }
      },
    },
  });

  private getLoginError(error: unknown): string {
    if (!(error instanceof HttpErrorResponse) || error.status === 0) {
      return 'Could not reach PropertyPro. Check that the API is running and try again.';
    }

    return error.error?.detail ?? error.error?.message ?? 'The email or password is incorrect.';
  }
}
