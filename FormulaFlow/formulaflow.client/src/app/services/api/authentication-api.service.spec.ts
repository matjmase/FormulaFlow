import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { AuthenticationApiService } from './authentication-api.service';
import { SessionDto } from '../../models/session-dto.model';

describe('AuthenticationApiService', () => {
  let service: AuthenticationApiService;
  let httpMock: HttpTestingController;

  const credentials = { email: 'test@example.com', password: 'P@ssw0rd!' };
  const session: SessionDto = { email: credentials.email, roles: ['User'] };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });

    service = TestBed.inject(AuthenticationApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should register a user', () => {
    service.register(credentials).subscribe((result) => {
      expect(result).toEqual(session);
    });

    const req = httpMock.expectOne('/api/Authentication/register');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(credentials);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(session);
  });

  it('should log in a user', () => {
    service.login(credentials).subscribe((result) => {
      expect(result).toEqual(session);
    });

    const req = httpMock.expectOne('/api/Authentication/login');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(credentials);
    expect(req.request.withCredentials).toBeTrue();
    req.flush(session);
  });

  it('should log out the current user', () => {
    service.logout().subscribe((result) => {
      expect(result).toBeNull();
    });

    const req = httpMock.expectOne('/api/Authentication/logout');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toBeNull();
    expect(req.request.withCredentials).toBeTrue();
    req.flush(null);
  });
});
