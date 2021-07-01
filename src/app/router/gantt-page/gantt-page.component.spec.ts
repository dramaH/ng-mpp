import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GanttPageComponent } from './gantt-page.component';

describe('GanttPageComponent', () => {
  let component: GanttPageComponent;
  let fixture: ComponentFixture<GanttPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GanttPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GanttPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
