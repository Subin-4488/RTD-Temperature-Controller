import { TestBed } from '@angular/core/testing';

import { ManualmodeService } from './manualmode.service';

describe('ManualmodeService', () => {
  let service: ManualmodeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManualmodeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
