import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
@Component({
  selector: 'app-resize-bar',
  templateUrl: './resize-bar.component.html',
  styleUrls: ['./resize-bar.component.scss']
})
export class ResizeBarComponent implements OnInit {

  // between-hor//两个水平
  // between-ver//两个垂直
  // single-hor
  // single-ver
  // single-width
  // single-height
  @Input() public lazyMove = false;
  @Input() public mode: string;
  @Input() public before: any;
  @Input() public after: any;
  @Input() public single: any;
  @Input() public single_width: any;
  @Output() resizeCallback: EventEmitter<{ x: number, y: number }> = new EventEmitter();



  childResizeLeft: any = -5;
  childOpacity = 0;
  public get resizeChildStyle() {
    return {
      left: this.childResizeLeft + 'px',
      opacity: this.childOpacity
    };
  }

  public constructor() { }

  public ngOnInit() {
  }

  public mousedown(event) {
    event = event || window.event;
    if (event == null) {
      return;
    }
    event.
      event.stopPropagation();
    let preX = event.x;
    let preY = event.y;
    const startX = event.x;
    const startY = event.y;
    document.onmousemove = (evtMove: any) => {
      evtMove = evtMove || window.event;
      if (evtMove == null) {
        return;
      }
      const moveX = evtMove.x - preX;
      const moveY = evtMove.y - preY;
      this.resizing(moveX, moveY);
      preX = evtMove.x;
      preY = evtMove.y;
    };
    document.onmouseup = (evtUp: any) => {
      const moveX = evtUp.x - startX;
      const moveY = evtUp.y - startY;
      document.onmousemove = null;
      document.onmouseup = null;
      this.resizeCallback.emit({ x: moveX, y: moveY });
    };
  }

  /**
   * 懒拖拽
   * 先移动resize-bar，再改变两边的容器宽度
   * @param event any
   */
  public lazyMoveMouseDown(event) {
    event = event || window.event;
    if (event == null) {
      return;
    }
    event.stopPropagation();
    const startX = event.x;
    const startY = event.y;
    let preX = event.x;
    let preY = event.y;
    document.onmousemove = (evtMove: any) => {
      evtMove.stopPropagation();
      evtMove = evtMove || window.event;
      if (evtMove == null) {
        return;
      }
      const moveX = evtMove.x - preX; // X轴方向相对前一次位置的偏移量

      this.childResizeLeft += moveX;
      this.childOpacity = 0.3;
      preX = evtMove.x;
      preY = evtMove.y;
    };
    document.onmouseup = (evtUp: any) => {
      const moveX = evtUp.x - startX;
      const moveY = evtUp.y - startY;
      this.resizing(moveX, moveY);
      this.childResizeLeft = -5;
      this.childOpacity = 0;
      document.onmousemove = null;
      document.onmouseup = null;
      this.resizeCallback.emit({ x: moveX, y: moveY });

    };
  }

  public resizing(moveX, moveY) {
    if (this.mode === 'between-hor' || this.mode === 'between-ver') {
      this.resizeBoth(moveX, moveY);
    }
    if (this.mode === 'single-height' || 'single-width') {
      this.resizeSingle(moveX, moveY);
    }
  }

  /**
   * 改变两侧容器
   * @param moveX 宽度改变的值
   * @param moveY 高度改变的值
   */
  public resizeBoth(moveX, moveY) {
    if (this.before == null || this.after == null) {
      return;
    }
    if (this.mode === 'between-hor') {
      const beforeW = this.before.width + moveX;
      const afterW = this.after.width - moveX;
      if (beforeW > 0 && afterW > 0) {
        this.before.width = beforeW;
        this.after.width = afterW;
      }
    }
    if (this.mode === 'between-ver') {
      const beforeH = this.before.height + moveY;
      const afterH = this.after.height - moveY;
      if (beforeH > 0 && afterH > 0) {
        this.before.height = beforeH;
        this.after.height = afterH;
      }
    }
  }

  /**
   * 只改变一侧容器
   * @param moveX 宽度改变的值
   * @param moveY 高度改变的值
   */
  public resizeSingle(moveX, moveY) {
    if (this.single == null) {
      return;
    }
    if (this.mode === 'single-height') {
      const height = this.single.height - moveY;
      if (height > 0) {
        if (this.single.setHeight) {
          this.single.setHeight(height);
        } else {
          this.single.height = height;
        }
      }
    }
    if (this.mode === 'single-width') {
      const width = this.single.width - moveX;
      if (width > 0) {
        if (this.single.setWidth) {
          this.single.setWidth(width);
        } else {
          this.single.width = width;
        }
      }
    }
  }
}
