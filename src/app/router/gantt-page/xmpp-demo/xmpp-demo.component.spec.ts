import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { XmppDemoComponent } from './xmpp-demo.component';

describe('XmppDemoComponent', () => {
  let component: XmppDemoComponent;
  let fixture: ComponentFixture<XmppDemoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XmppDemoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XmppDemoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
