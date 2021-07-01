import { Component, OnInit, Input, TemplateRef, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';

export interface IresizeInitStyle {
  width: number;
  height: number;
  left: number;
  top: number;
}

@Component({
  selector: 'zzj-resize-box',
  templateUrl: './resize-box.component.html',
  styleUrls: ['./resize-box.component.less']
})
export class ResizeComponent implements OnInit {
  @ViewChild('box', { static: false }) box: ElementRef;
  @Input() initStyle: IresizeInitStyle;
  @Output() resizeEvent: EventEmitter<IresizeInitStyle> = new EventEmitter<IresizeInitStyle>();

  public chatStyleModel: any = {
    top: 0,
    left: 0,
  };
  public get chatStyle() {
    return {
      width: this.chatStyleModel.width + 'px',
      height: this.chatStyleModel.height + 'px',
      left: this.chatStyleModel.left + 'px',
      top: this.chatStyleModel.top + 'px'
    };
  }


  constructor() {

  }

  ngOnInit() {
    this.chatStyleModel = this.initStyle;
  }

  public ngAfterViewInit() {

  }

  public adjustPanel(event: any, styleModel: any, type: string, top?: boolean, bottom?: boolean, left?: boolean, rigth?: boolean) {
    event = event || window.event;
    if (event == null) {
      return;
    }
    event.stopPropagation();
    let preX = event.x;
    let preY = event.y;
    document.onmousemove = (evtMove: any) => {
      evtMove = evtMove || window.event;
      if (evtMove == null) {
        return;
      }
      const moveX = evtMove.x - preX;
      const moveY = evtMove.y - preY;

      if (type === 'move') {
        styleModel.left = styleModel.left + moveX;
        styleModel.top = styleModel.top + moveY;
      }
      if (type === 'resize') {
        if (top) {
          styleModel.top = styleModel.top + moveY;
          styleModel.height = styleModel.height - moveY;
        }
        if (bottom) {
          styleModel.height = styleModel.height + moveY;
        }
        if (rigth) {
          styleModel.width = styleModel.width + moveX;
        }
        if (left) {
          styleModel.left = styleModel.left + moveX;
          styleModel.width = styleModel.width - moveX;
        }

      }
      preX = evtMove.x;
      preY = evtMove.y;
    };
    document.onmouseup = (evtUp: any) => {

      if (type === 'resize') {
        this.resizeEvent.emit(styleModel);
      }
      document.onmousemove = null;
      document.onmouseup = null;
      preX = 0;
      preY = 0;
    };
  }
}
