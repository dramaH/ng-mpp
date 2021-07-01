import { Component, OnInit, ViewContainerRef, ViewChild, TemplateRef, AfterViewInit, Input, EventEmitter, Output, SimpleChanges, ComponentRef, ChangeDetectorRef, ChangeDetectionStrategy, ViewEncapsulation, ElementRef } from '@angular/core';
import { OverlayConfig, Overlay, OverlayRef, GlobalPositionStrategy } from '@angular/cdk/overlay';
import { TemplatePortalDirective } from '@angular/cdk/portal';
import { isNullOrUndefined } from 'util';
import { Router, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs/operators';
// import { ModalComponent } from './modal/modal.component';

@Component({
  selector: 'zzj-modal',
  templateUrl: './zzj-modal.component.html',
  styleUrls: ['./zzj-modal.component.scss']
})
export class ZzjModalComponent<T = any, R = any> implements AfterViewInit {
  @Input() public nzOkLoading = false;
  @Input() public zzVisible = false;
  @Input() public zzTitle: string | TemplateRef<{}>; // 标题 | header模板
  @Input() public zzContent: TemplateRef<void>; // content模板
  @Input() public zzFooter: TemplateRef<void>;  // footer模板
  @Input() public zzRemovable = false;  // 是否可移动的弹框
  @Input() public zzResizable = false;  // 是否可移动的弹框
  @Input() public zzMaskClosable = false;   // 是否点击背景板关闭
  @Input() public zzShowMask = false;
  @Input() public zzClassName: string;  // 弹框最外层class
  @Input() public zzTop: number;  // 顶部距离
  @Input() public zzLeft: number; // ...
  @Input() public zzBottom: number; // ...
  @Input() public zzRight: number; // ...
  @Input() public zzWidth: number | string; // 弹窗宽度
  @Input() public zzHeight: number | string;  // 弹窗高度
  @Input() public showIcon = false; // 扩大/缩小图标
  @Output() public readonly zzAfterOpen = new EventEmitter<void>(); // 弹窗弹出事件
  @Output() public readonly zzAfterClose = new EventEmitter<void>();  // 弹窗摧毁事件
  @Output() public readonly zzOnCancel = new EventEmitter<void>();  // 关闭事件
  @Output() public readonly zzOnOk = new EventEmitter<void>();  // 关闭事件
  @Output() public readonly zzKeyboard = new EventEmitter<number>();  // 键盘事件，监听keycode
  @Output() public readonly zzOnResize = new EventEmitter<void>();  // 缩放事件
  @Output() public zzExpandOrNarrow = new EventEmitter<any>(); // 扩大/缩小事件
  @ViewChild('overlayGlobalTemplate', { static: false }) public templateGlobalPortals: TemplatePortalDirective;
  @ViewChild('zzModalComponent', { static: false }) public zzModalComponent: any;
  @ViewChild('zzModal', { static: false }) public zzModal: ElementRef;
  @ViewChild('zzHeader', { static: false }) public zzHeader: ElementRef;
  public overlayRef: OverlayRef;
  public chatStyleModel: any = {
  };
  public get chatStyle() {
    return {
      width: this.chatStyleModel.width + 'px',
      height: this.chatStyleModel.height + 'px',
      left: this.chatStyleModel.left + 'px',
      top: this.chatStyleModel.top + 'px'
    };
  }
  public nzType = true; // 图标
  public constructor(public overlay: Overlay, private router: Router) {

  }

  public ngAfterViewInit() {
    // 路由跳转，移除弹窗
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(() => this.router)
      )
      .subscribe((event) => {
        if (this.overlayRef) {
          this.overlayRef.dispose()
        }

      });
  }

  /**
   * modal初始化
   */
  initModal() {
    // 初始化chatStyleModel
    // console.log(this.zzModal.nativeElement.clientWidth)
    // console.log(this.zzModal.nativeElement.clientHeight)
    if (this.zzResizable) {
      this.chatStyleModel.width = this.zzModal.nativeElement.clientWidth;
      this.chatStyleModel.height = this.zzModal.nativeElement.clientHeight;
    }


    // 改变header鼠标样式
    if (!this.zzHeader || !this.zzHeader.nativeElement) {
      return;
    }
    const oldStyle = this.zzHeader.nativeElement.getAttribute('style');
    if (this.zzRemovable) {
      this.zzHeader.nativeElement.setAttribute('style', `cursor: move`);
    } else {
      if (oldStyle) {
        const newStyle = oldStyle.replace('cursor: move', '');
        this.zzHeader.nativeElement.setAttribute('style', newStyle);
      }

    }
  }

  public ngOnChanges(changes: SimpleChanges): void {
    if (changes.zzVisible) {
      this.handleVisibleStateChange(this.zzVisible, !changes.zzVisible.firstChange);
    }
  }

  private handleVisibleStateChange(visible: boolean, animation = true) {
    if (visible) {
      this.showOverlayGlobalPanelCenter();
      setTimeout(() => {
        // header设置鼠标样式
        this.initModal();
      }, 500);
      this.zzAfterOpen.emit();
    } else if (this.overlayRef) {
      this.overlayRef.dispose();
      this.zzAfterClose.emit();
    }
  }

  public closeModal() {
    this.zzOnCancel.emit();
  }

  public clickOnOk() {
    this.zzOnOk.emit();
  }

  public clickOnCancel() {
    this.zzOnCancel.emit();
  }

  public isNonEmptyString(value: {}): boolean {
    return typeof value === 'string' && value !== '';
  }

  public isTemplateRef(value: {}): boolean {
    return value instanceof TemplateRef;
  }

  public adjustPanel(event: any, styleModel: any, type: string, top?: boolean, bottom?: boolean, left?: boolean, rigth?: boolean) {
    if (!this.zzRemovable) {
      return;
    }
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
          styleModel.left = styleModel.left;
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
      document.onmousemove = null;
      document.onmouseup = null;
      preX = 0;
      preY = 0;
      if (type === 'resize') {
        this.zzOnResize.emit();
      }
    };
  }


  /**
   * overlay 在整个屏幕的中间显示
   */
  public showOverlayGlobalPanelCenter() {
    if (!this.templateGlobalPortals) {
      return;
    }
    // config: OverlayConfig overlay的配置，配置显示位置，和滑动策略
    const config = new OverlayConfig();
    // config.positionStrategy = this.overlay.position().global()
    const postion: GlobalPositionStrategy = this.overlay.position().global();
    if (!isNullOrUndefined(this.zzLeft)) {
      postion.left(this.zzLeft + 'px');
    } else {
      if (!this.zzResizable) {
        // postion.centerHorizontally();
      }
      postion.centerHorizontally();
    }

    if (!isNullOrUndefined(this.zzBottom)) {
      postion.bottom(this.zzBottom + 'px');
    }
    if (!isNullOrUndefined(this.zzTop)) {
      postion.top(this.zzTop + 'px');
    }
    if (isNullOrUndefined(this.zzBottom) && isNullOrUndefined(this.zzTop)) {

      postion.centerVertically();

    }
    if (!isNullOrUndefined(this.zzRight)) {
      postion.right(this.zzRight + 'px');
    }
    if (!isNullOrUndefined(this.zzWidth)) {
      if (typeof this.zzWidth === 'number') {
        postion.width(this.zzWidth + 'px');
      } else {
        postion.width(this.zzWidth);
      }
    }
    if (!isNullOrUndefined(this.zzHeight)) {
      if (typeof this.zzHeight === 'number') {
        postion.height(this.zzHeight + 'px');
      } else {
        postion.height(this.zzHeight);
      }
    }
    // 垂直居中
    // .left(`${this.globalOverlayPosition}px`) // 自己控制位置

    config.positionStrategy = postion;
    // 是否显示背景层
    config.hasBackdrop = this.zzShowMask; // 设置overlay后面有一层背景, 当然你也可以设置backdropClass 来设置这层背景的class
    config.backdropClass = 'backdrop';
    this.overlayRef = this.overlay.create(config); // OverlayRef, overlay层

    this.overlayRef.backdropClick().subscribe(() => {
      // 点击了backdrop背景
      if (this.zzMaskClosable) {
        this.zzOnCancel.emit();
      }
    });
    // OverlayPanelComponent是动态组件
    // 创建一个ComponentPortal，attach到OverlayRef，这个时候我们这个overlay层就显示出来了。
    // overlayRef.attach(new ComponentPortal(ModalComponent, this.vcRef));
    // this.overlayRef.overlayElement.style.zIndex = '2001';
    this.overlayRef.attach(this.templateGlobalPortals);
    this.chatStyleModel.left = this.overlayRef.overlayElement.offsetLeft;
    this.chatStyleModel.top = this.overlayRef.overlayElement.offsetTop;
    // 监听overlayRef上的键盘按键事件
    this.overlayRef.keydownEvents().subscribe((event: KeyboardEvent) => {
      this.zzKeyboard.emit(event.keyCode);
    });
  }

  // 扩大/缩小
  public expandOrNarrow() {
    this.nzType = !this.nzType;
    this.zzExpandOrNarrow.emit(this.nzType);
  }
}
