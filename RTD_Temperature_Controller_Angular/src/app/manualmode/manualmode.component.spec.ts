import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManualmodeComponent } from './manualmode.component';

describe('ManualmodeComponent', () => {
  let component: ManualmodeComponent;
  let fixture: ComponentFixture<ManualmodeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ManualmodeComponent]
    });
    fixture = TestBed.createComponent(ManualmodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
