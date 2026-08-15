import {
  Directive,
  Input,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { SessionLocalStorageService } from '../services/session-local-storage.service';

@Directive({
  selector: '[appAuthenticatedIf]',
  standalone: false,
})
export class AuthenticatedIfDirective implements OnInit, OnDestroy {
  private hasView = false;
  private expectedAuthentication = true;

  private subscription: Subscription | null = null;

  constructor(
    private templateRef: TemplateRef<unknown>,
    private viewContainer: ViewContainerRef,
    private sessionStorage: SessionLocalStorageService,
  ) {}
  ngOnInit(): void {
    this.subscription = this.sessionStorage.sessionChanged.subscribe(() =>
      this.updateView(),
    );
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  @Input()
  set appAuthenticatedIf(value: boolean | string | null | undefined) {
    this.expectedAuthentication = this.toBoolean(value);
    this.updateView();
  }

  private updateView(): void {
    const isAuthenticated = this.sessionStorage.getSession() !== null;
    const shouldDisplay = isAuthenticated === this.expectedAuthentication;

    if (shouldDisplay && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
      return;
    }

    if (!shouldDisplay && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }

  private toBoolean(value: boolean | string | null | undefined): boolean {
    if (typeof value === 'boolean') {
      return value;
    }

    if (value === null || value === undefined || value === '') {
      return true;
    }

    return value.toLowerCase() !== 'false';
  }
}
