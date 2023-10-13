import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserOrdersDialogComponent } from './user-orders-dialog.component';

describe('UserOrdersDialogComponent', () => {
  let component: UserOrdersDialogComponent;
  let fixture: ComponentFixture<UserOrdersDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserOrdersDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserOrdersDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
