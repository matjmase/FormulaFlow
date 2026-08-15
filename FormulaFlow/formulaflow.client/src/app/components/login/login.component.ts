import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthenticationApiService } from '../../services/api/authentication-api.service';
import { SessionLocalStorageService } from '../../services/session-local-storage.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.Eager,
})
export class LoginComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthenticationApiService);
  private readonly sessionStorage = inject(SessionLocalStorageService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  public loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  public isSubmitting = false;

  public submit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const { email, password } = this.loginForm.getRawValue();

    this.isSubmitting = true;
    this.authService
      .login({ email, password })
      .pipe(finalize(() => (this.isSubmitting = false)))
      .subscribe({
        next: (session) => {
          this.sessionStorage.setSession(session);
          this.snackBar.open('Logged in successfully.', 'Close', {
            duration: 4000,
          });
          this.router.navigate(['/']);
        },
        error: () => {
          this.snackBar.open('Login failed. Please try again.', 'Close', {
            duration: 4000,
          });
        },
      });
  }
}
