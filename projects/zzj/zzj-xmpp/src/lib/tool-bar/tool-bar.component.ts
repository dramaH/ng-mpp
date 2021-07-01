import { Component, OnInit, Input } from '@angular/core';
import { NzNotificationService, NzModalService, NzMessageService } from 'ng-zorro-antd';
import moment from 'moment';
import { EXTENDATTRS, Xmpp, XmppTask } from '../src/api-public';
import { GanttHelperService } from '../gantt-chart-service/gantt-helper.service';
import { GanttRequestService } from '../gantt-chart-service/gantt-request.service';
import { GanttSize } from '../gantt-chart-service/gantt.config';
declare var Neon: any;
@Component({
  selector: 'app-tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.scss']
})


export class ToolBarComponent implements OnInit {
  @Input() Xmpp: Xmpp;
  public ganttChecked = false;
  public selector: any;
  public _percent = 0;
  public interval: any;
  public simulate = false;
  public playdisabled = false;
  public pausedisabled = true;
  public startDate = null;
  public showStart = null;
  public endDate = null;
  public finishDate = null;
  public diffDate = null;
  public simulateVisible = false;
  public calenderVisible = false;

  public perStep: any;

  public currentStep = 0;
  public uuids: any = [];

  public playTasks: any = [];
  public permission: any;
  public constructor(
    private _notification: NzNotificationService,
    private confirmServ: NzModalService,
    private message: NzMessageService,
    private ganttHelpServ: GanttHelperService,
    private ganttRequestSev: GanttRequestService
  ) {
  }

  public createNotification = (type, title, message) => {
    this._notification.create(type, title, message);
  }
  public ngOnInit() {
    // this.selector = Neon.getSelector();

  }

  public transformDate(value: any) {
    if (typeof value === 'number') {
      return moment.unix(value);
    } else {
      return value;
    }
  }

  // 日历设置
  public calenderShow() {
    this.calenderVisible = true;
  }
  public calenderCancel() {
    this.calenderVisible = false;
  }




  // }
  // 保存allTasks
  public saveTasks(type: string) {
    // this.ganttHelpServ.saveTasksHanle(this.Xmpp, type);
  }

  // 跟踪横道图
  public ganttTracking(e) {

  }

  // 插入新的task
  // public addTask() {
  //   this.Xmpp.task.addTaskHandle();
  // }

  // 删除选中元素
  public deleteTask() {
    const that = this;
    this.confirmServ.confirm({
      nzTitle: '是否要删除选中任务及其所有的子任务？？',
      nzContent: '点击确认将删除选中任务及其所有的子任务，点击取消将不删除任何任务。',
      nzOnOk() {
        that.deleteTaskHandle();
      }
    });

  }

  deleteTaskHandle() {
    const deleteTasks = [];
    const search = (element) => {
      if (element.childTaskID && element.childTaskID.length > 0) {
        element.childTaskID.forEach((childID) => {
          if (deleteTasks.indexOf(childID) === -1) {
            deleteTasks.push(childID);
          }
          search(this.Xmpp.allTasks[childID - 1]);
        });
      } else {
        return;
      }
    };
    for (const element of this.Xmpp.allTasks) {
      const id = element.id;
      if (element.isSelected) {
        deleteTasks.push(id);
        search(element);
        // allTasks.splice(id - 1, 1)
      }
    }

    // 将删除的task放在this.alreadyDeleteTasks中
    for (const id of deleteTasks) {
      const finder = this.Xmpp.allTasks.find((task) => {
        return task.id === id;
      });
      if (finder.sqId) {
        // this.ganttHelpServ.alreadyDeleteTasks.push(finder.sqId);
      }
    }

    this.Xmpp.task.deleteTaskHandle(deleteTasks);

  }

  // 降级
  public depressTaskLevel() {
    const tasks = this.checkSelectNumber();
    this.Xmpp.task.depressTaskLevel(tasks);
  }

  // //升级
  public promoteTaskLevel() {
    const tasks = this.checkSelectNumber();
    this.Xmpp.task.promoteTaskLevel(tasks);
  }

  checkSelectNumber() {
    const selectNumber = [];
    this.Xmpp.allTasks.forEach((element) => {
      if (element.isSelected) {
        selectNumber.push(element);
      }
    });
    return selectNumber;
  }


  // // 新增里程碑
  // public addMilepost() {
  //   this.Xmpp.task.addTaskHandle(new XmppTask({ isMilepost: true }));
  // }

  /**
   * 绑定构件
   * @param isFirstBind
   */
  // public async bingdingGUID(isFirstBind: boolean) {
  //   let uuids: string[];
  //   const selectTasks = this.checkSelectNumber();
  //   const task = selectTasks[0];
  //   const ganttId = this.Xmpp.currentGantt.id;
  //   const extend = await this.ganttRequestSev.getTaskAttrs(ganttId, task.sqId);
  //   const finder = extend.find((attr) => {
  //     return attr.fieldID === EXTENDATTRS.binding.FieldID;
  //   });
  //   if (isFirstBind) {
  //     uuids = this.ganttApi.getSelectedUUIDs();
  //   } else {
  //     // 追加构件
  //     uuids = this.ganttApi.addOver();
  //   }
  //   if (uuids.length === 0) {
  //     this.createNotification('error', '请选择构件', 'small');
  //     return;
  //   }
  //   if (selectTasks.length !== 1) {
  //     this.createNotification('error', '请选择单个任务绑定构件', 'small');
  //   }
  //   const bingUUids = [];


  //   // selectTasks[0].bindings = uuid;
  //   uuids.forEach((uid) => {
  //     bingUUids.push({
  //       uuid: gum.btoa(uid),
  //       isFinished: false
  //     });
  //   });



  //   if (!task.extendAttrs) {
  //     task.extendAttrs = {};
  //     task.extendAttrs.uuids = bingUUids;
  //     const param = {
  //       FieldID: EXTENDATTRS.binding.FieldID,
  //       Value: JSON.stringify(task.extendAttrs)
  //     };
  //     const res = this.ganttRequestSev.bindTaskExtendedAttribute(ganttId, task.sqId, param);
  //     if (res) {
  //       this.message.success('绑定成功');
  //     } else {
  //       this.message.success('绑定失败');
  //     }
  //   } else {
  //     task.extendAttrs.uuids = bingUUids;
  //     const param = {
  //       Value: JSON.stringify(task.extendAttrs)
  //     };
  //     const res = await this.ganttRequestSev.updateExtendedAttrbute(ganttId, task.sqId, finder.id, param);
  //     if (res) {
  //       this.message.success('更新绑定成功');
  //     } else {
  //       this.message.success('更新绑定失败');
  //     }
  //   }
  //   // let symbol = selectTasks[0].symbol;
  //   // let extendAttrs = Gantt.mpp.extraAttrMap.get(symbol);
  //   // Object.assign(extendAttrs, { bindings });
  //   // Gantt.mpp.extraAttrMap.set(symbol, extendAttrs);


  // }

  /**
   * 解绑构件
   */
  // public async unbingdingGUID() {
  //   const selectTasks = this.checkSelectNumber();
  //   const ganttId = this.Xmpp.currentGantt.id;
  //   for (let i = 0; i < selectTasks.length; i++) {
  //     const task: GanttModel = selectTasks[i];
  //     if (!task.extendAttrs) {
  //       continue;
  //     }

  //     if (!task.extendAttrs.uuids) {
  //       return;
  //     }

  //     delete task.extendAttrs.uuids;
  //     const param = {
  //       Value: JSON.stringify(task.extendAttrs)
  //     };
  //     const res = await this.ganttRequestSev.updateExtendedAttrbute(ganttId, task.sqId, task.extendAttrsId, param);
  //     if (res) {
  //       this.message.success('解绑成功');
  //     } else {
  //       this.message.success('解绑失败');
  //     }
  //   }

  //   // this.modelService.selectFloorShow()
  //   this.ganttApi.clearSelectedUUIDS();
  // }

  ///////////////////////////////////// 施工模拟 ////////////////////////////////////

  // 退出模拟
  public simulateHide() {
    this.simulate = false;
    // this.modelService.selectFloorShow()
  }

  public refresh() {
    this.Xmpp.render();
  }

  public compress() {
    const baseWidth = GanttSize.calenderBaseWidth;
    if (baseWidth > 5) {
      GanttSize.calenderBaseWidth = baseWidth - 5;
      this.Xmpp.draw.updateCanvasInfo();
    } else {
      this.message.error('不能再缩小了');
    }
  }

  public expand() {
    const baseWidth = GanttSize.calenderBaseWidth;
    if (baseWidth < 30) {
      GanttSize.calenderBaseWidth = baseWidth + 5;
      this.Xmpp.draw.updateCanvasInfo();
    } else {
      this.message.error('不能再放大了');
    }
  }

  public reload() {
    this._percent = 0;
    this.currentStep = 0;
    this.playTasks = [];
    this.playdisabled = false;
    this.pausedisabled = true;
  }

  public increase() {
    if (this.interval) {
      clearInterval(this.interval);
    }
    console.log(this.playTasks);
    if (this.currentStep < this.playTasks.length) {
      this._percent = this._percent + this.perStep;
      this.currentStep++;
      // this.render();
    } else {
      this._percent = 100;
    }
    this.playdisabled = false;
    this.pausedisabled = true;
  }

  public decline() {
    if (this.interval) {
      clearInterval(this.interval);
    }
    if (this.currentStep > 0) {
      this.remove();
      this._percent = this._percent - this.perStep;
      this.currentStep--;
    } else {
      this.currentStep = 0;
      this._percent = 0;
    }
    this.playdisabled = true;
    this.pausedisabled = false;
  }

  // public play() {
  //   this.playdisabled = true;
  //   this.pausedisabled = false;
  //   // let isPlanning = this.ganttPermission.isPlanning;
  //   this.interval = setInterval(() => {
  //     if (this.currentStep < this.playTasks.length) {
  //       this._percent = this._percent + this.perStep;
  //       this.currentStep++;
  //       this.render();
  //     } else {
  //       this._percent = 100;
  //       this.currentStep = this.playTasks.length;
  //       clearInterval(this.interval);
  //       return;
  //     }
  //   }, 1000);
  // }


  public pause() {
    if (this.interval) {
      clearInterval(this.interval);
      this.pausedisabled = true;
      this.playdisabled = false;
    }
  }

  public remove() {
    const currentStep = this.currentStep;
    const isPlanning = false;
    if (isPlanning) {
      const uuidItem = this.playTasks[currentStep - 1];
      const finishAt = uuidItem.finishAt;
      // 该构件完成时间
      this.finishDate = moment.unix(finishAt).format('YYYY-MM-DD');
      // 构件时差
      this.diffDate = moment.unix(finishAt).diff(moment(this.startDate), 'days');
    } else {
      const task = this.playTasks[currentStep - 1];
      const symbol = task.symbol;
      const endDate = task.endDate;
      // 该构件完成时间
      this.finishDate = moment(endDate).format('YYYY-MM-DD');
      // 构件时差
      this.diffDate = moment(endDate).diff(moment(this.startDate), 'days');
    }
  }

  // public render() {
  //   let currentStep = this.currentStep;
  //   let isPlanning = false;
  //   if (isPlanning) {
  //     let uuidItem = this.playTasks[currentStep - 1];
  //     let uuid = uuidItem.uuid;
  //     let finishAt = uuidItem.finishAt;
  //     this.ganttApi.renderUuid(uuid);
  //     // 该构件完成时间
  //     this.finishDate = moment.unix(finishAt).format('YYYY-MM-DD');
  //     // 构件时差
  //     this.diffDate = moment.unix(finishAt).diff(moment(this.startDate), 'days');
  //   } else {
  //     let task = this.playTasks[currentStep - 1];
  //     let symbol = task.symbol;
  //     let endDate = task.endDate;
  //     this.ganttApi.renderUuidsFromTask(symbol);
  //     // 该构件完成时间
  //     this.finishDate = moment(endDate).format('YYYY-MM-DD');
  //     // 构件时差
  //     this.diffDate = moment(endDate).diff(moment(this.startDate), 'days');
  //   }
  // }

  /*
  * 完成时间不能大于开始时间的
  * 禁用开始时间前的日期
   */

  public disabledEndDate = (endValue) => {
    if (!endValue || !this.startDate) {
      return false;
    }
    return endValue.getTime() <= this.startDate.getTime();
  }

  public showModal = () => {
    this.simulateVisible = true;
  }

  // public simulateOk = () => {
  //   console.log('点击了确定');
  //   this.reload();
  //   this.getAllplayTasks();
  //   this.Xmpp.extraInfo.detailView = false;
  //   this.showStart = moment(this.startDate).format('YYYY-MM-DD');
  // }

  public simulateCancel = (e) => {
    this.simulateVisible = false;
  }


  public getAllplayTasks() {
    const allTasks = this.Xmpp.allTasks;
    const isPlanning = false;
    const playTasks = [];
    const startUnix = moment(this.startDate).hours(0).minutes(0).seconds(0).unix();
    const endUnix = moment(this.endDate).hours(11).minutes(59).seconds(59).unix();
    if (isPlanning) {
      for (let i = 0; i < allTasks.length; i++) {
        const element = allTasks[i];
        const bindings = allTasks[i].bindings;
        if (bindings && bindings.length > 0) {
          bindings.forEach((item) => {
            const finishAt = item.finishAt;
            const uuid = item.uuid;
            if (playTasks.indexOf(item) === -1 && finishAt && finishAt >= moment(this.startDate).unix() && finishAt <= moment(this.endDate).unix()) {
              playTasks.push(item);
            }
          });
        }
      }
      if (playTasks.length > 0) {
        this.playTasks = playTasks;
        this.simulateVisible = false;
        this.perStep = Number((100 / playTasks.length).toFixed(2));
        // 显示进度条
        this.simulate = true;
      } else {
        this.createNotification('error', '请检查', '所选时间段没有绑定的构件');
        return;
      }
    } else {
      // 拼装playTasks
      const playTasks = [];
      const minStart = startUnix;
      for (let i = 0; i < allTasks.length; i++) {
        const element = allTasks[i];
        const task_Start = element.startDate ? moment(element.startDate).unix() : 0;
        const task_End = element.startDate ? moment(element.endDate).unix() : 0;
        if (
          task_Start && task_End &&
          task_Start >= startUnix &&
          task_End <= endUnix &&
          element.bindings && element.bindings.length > 0
        ) {
          playTasks.push(element);
        }
      }

      // playTasks按计划开始时间排序
      for (let i = 0; i < playTasks.length; i++) {
        for (let j = i + 1; j < playTasks.length; j++) {
          if (moment(playTasks[i].startDate).unix() > moment(playTasks[j].startDate).unix()) {
            const tmp = playTasks[i];
            playTasks[i] = playTasks[j];
            playTasks[j] = tmp;
          }
        }
      }

      if (playTasks.length > 0) {
        this.playTasks = playTasks;
        this.simulateVisible = false;
        this.perStep = Math.ceil(100 / this.playTasks.length);
        // 显示进度条
        this.simulate = true;
      } else {
        this.createNotification('error', '请检查', '所选时间段没有绑定的构件');
        return;
      }


    }

    // this.uuids = uuids;
  }



}
