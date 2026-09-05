import { ComponentFixture, TestBed } from '@angular/core/testing';
import { VersionDIff } from './diff';

describe('VersionDIff', () => {
  let component: VersionDIff;
  let fixture: ComponentFixture<VersionDIff>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VersionDIff],
    }).compileComponents();

    fixture = TestBed.createComponent(VersionDIff);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
