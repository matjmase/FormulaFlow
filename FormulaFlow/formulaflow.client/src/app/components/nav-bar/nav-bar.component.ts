import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthenticationApiService } from '../../services/api/authentication-api.service';
import { SessionLocalStorageService } from '../../services/session-local-storage.service';

@Component({
  selector: 'app-nav-bar',
  standalone: false,
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.scss',
})
export class NavBarComponent implements OnInit {
  public readonly rippleColor = 'rgba(0, 0, 0, 0.4)';

  public authenticatedLinks: NavBarLink[] = [
    { label: 'Strategy Manager', icon: 'insights', url: '/strategy-manager' },
    { label: 'Stock Manager', icon: 'manage_accounts', url: '/stock-manager' },
  ];

  public unAuthenticatedLinks: NavBarLink[] = [
    { label: 'Login', icon: 'login', url: '/login' },
    { label: 'Register', icon: 'person_add', url: '/register' },
  ];

  public sidenavOpen = false;

  constructor(
    private authService: AuthenticationApiService,
    private router: Router,
    private sessionStorage: SessionLocalStorageService,
    private snackBar: MatSnackBar,
  ) {}

  ngOnInit() {}

  toggleSidenav() {
    this.sidenavOpen = !this.sidenavOpen;
  }

  closeSidenav() {
    this.sidenavOpen = false;
  }

  logout() {
    this.authService.logout().subscribe({
      next: () => {
        this.closeSidenav();
        this.sessionStorage.clearSession();
        this.snackBar.open('Logged out successfully.', 'Close', {
          duration: 4000,
        });
        this.router.navigate(['/']);
      },
      error: () => {
        this.closeSidenav();
        this.sessionStorage.clearSession();
        this.snackBar.open('Logout failed. Please try again.', 'Close', {
          duration: 4000,
        });
        this.router.navigate(['/']);
      },
    });
  }
}

interface NavBarLink {
  label: string;
  icon: string;
  url: string;
}
