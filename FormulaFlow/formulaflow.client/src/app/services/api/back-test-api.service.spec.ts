import { TestBed } from '@angular/core/testing';

import { BackTestApiService } from './back-test-api.service';

describe('BackTestApiService', () => {
  let service: BackTestApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BackTestApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
