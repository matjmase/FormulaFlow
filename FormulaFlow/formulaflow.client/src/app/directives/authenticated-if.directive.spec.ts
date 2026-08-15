import { AuthenticatedIfDirective } from './authenticated-if.directive';

describe('AuthenticatedIfDirective', () => {
  it('should create an instance', () => {
    const directive = new AuthenticatedIfDirective();
    expect(directive).toBeTruthy();
  });
});
