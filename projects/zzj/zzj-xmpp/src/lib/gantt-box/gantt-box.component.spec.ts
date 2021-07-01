import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GanttBoxComponent } from './gantt-box.component';

describe('GanttBoxComponent', () => {
  let component: GanttBoxComponent;
  let fixture: ComponentFixture<GanttBoxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GanttBoxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GanttBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });
});
