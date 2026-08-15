import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  Validators,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthenticationApiService } from '../../services/api/authentication-api.service';
import { SessionLocalStorageService } from '../../services/session-local-storage.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthenticationApiService);
  private readonly sessionStorage = inject(SessionLocalStorageService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  public registerForm = this.formBuilder.nonNullable.group(
    {
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: RegisterComponent.passwordsMatch },
  );

  public isSubmitting = false;

  public submit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const { email, password } = this.registerForm.getRawValue();

    this.isSubmitting = true;
    this.authService
      .register({ email, password })
      .pipe(finalize(() => (this.isSubmitting = false)))
      .subscribe({
        next: (session) => {
          this.sessionStorage.setSession(session);
          this.snackBar.open('Registered successfully.', 'Close', {
            duration: 4000,
          });
          this.router.navigate(['/']);
        },
        error: () => {
          this.snackBar.open(
            'Registration failed. Please try again.',
            'Close',
            {
              duration: 4000,
            },
          );
        },
      });
  }

  private static passwordsMatch(
    control: AbstractControl,
  ): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (!password || !confirmPassword || password === confirmPassword) {
      return null;
    }

    return { passwordsMismatch: true };
  }
}
