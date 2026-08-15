import {
  Directive,
  Input,
  OnDestroy,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { Subscription } from 'rxjs';
import { SessionLocalStorageService } from '../services/session-local-storage.service';

@Directive({
  selector: '[appAuthorizedIf]',
  standalone: false,
})
export class AuthorizedIfDirective implements OnDestroy {
  private hasView = false;
  private requiredRole: string | null = null;
  private sub: Subscription;

  constructor(
    private templateRef: TemplateRef<unknown>,
    private viewContainer: ViewContainerRef,
    private sessionStorage: SessionLocalStorageService,
  ) {
    this.sub = this.sessionStorage.sessionChanged.subscribe(() =>
      this.updateView(),
    );
  }

  @Input()
  set appAuthorizedIf(role: string | null | undefined) {
    this.requiredRole = role ?? null;
    this.updateView();
  }

  private updateView(): void {
    const session = this.sessionStorage.getSession();
    const hasRole = !!(
      session &&
      Array.isArray(session.roles) &&
      this.requiredRole &&
      session.roles.includes(this.requiredRole)
    );

    if (hasRole && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
      return;
    }

    if (!hasRole && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }
}
