import { TestBed } from '@angular/core/testing';

import { SessionLocalStorageService } from './session-local-storage.service';

describe('SessionLocalStorageService', () => {
  let service: SessionLocalStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SessionLocalStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
