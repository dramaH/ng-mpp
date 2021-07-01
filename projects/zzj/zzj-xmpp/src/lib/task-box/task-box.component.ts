import {
  Component,
  OnInit,
  Input,
  ElementRef,
  ViewChild,
  Output,
  EventEmitter,
  OnChanges,
} from '@angular/core';
import { NzNotificationService, NzMessageService } from 'ng-zorro-antd';
import {
  IXmpp,
  Xmpp,
  XmppColumn,
  PREVTYPE,
  XmppPredecessorLink,
  XmppTask,
  XmppAssignment,
} from '../src/api-public';
import { GanttHelperService } from '../gantt-chart-service/gantt-helper.service';
import { isNullOrUndefined } from 'util';
// import { GanttPermission } from '../../../services/gantt-chart/this.Xmpp.permission';
// import moment from 'moment';

class EditModel {
  public relations: XmppPredecessorLink[];
  public taskName: string;
  public assignmentsStr: string;
  public taskLevel: string;
  public isMilepost: boolean;
  public constructor(param) {
    param.relations && (this.relations = param.relations);
    param.taskName && (this.taskName = param.taskName);
    param.assignmentsStr && (this.assignmentsStr = param.assignmentsStr);
    param.taskLevel && (this.taskLevel = param.taskLevel);
    param.isMilepost && (this.isMilepost = param.isMilepost);
  }
}

declare var Neon: any;
@Component({
  selector: 'app-task-box',
  templateUrl: './task-box.component.html',
  styleUrls: ['./task-box.component.scss'],
})
export class TaskBoxComponent implements OnInit, OnChanges {
  @ViewChild('newTaskNameInput', { static: false })
  newTaskNameInput: ElementRef;
  @ViewChild('editInput', { static: false }) editInput: ElementRef;
  @Input() Xmpp: Xmpp;
  @Output() newTaskBlur: EventEmitter<void> = new EventEmitter<void>();
  public isVisible = false;
  public taskNameWidth = 100;
  public prevTaskWidth = 0;
  public headerWidth = 340;
  public otherWidthTotal = 800;
  public actualDisabled = true;
  public isAllselected = false;
  datepickVisible = false;
  // 前置任务编辑器

  public editPermission = true;
  columnList = [];

  newTask: any = {
    id: -1,
    taskName: '',
    startDate: null,
    endDate: null,
    taskLevel: '',
    isMilepost: false,
  };
  ctrlDown = false;
  editTaskId: number;
  settingModalView = false;
  editInfo: EditModel;
  PREVTYPE = PREVTYPE;
  settingTask: XmppTask;
  clickTimer: any = null;

  currentTask: XmppTask;
  currentKey: string;
  // currentId: any;
  public constructor(
    private _notification: NzNotificationService,
    private ganttHelpServ: GanttHelperService,
    private message: NzMessageService
  ) {
    console.log(this.Xmpp);
    const that = this;
    document.onkeydown = function(event) {
      const e = event || window.event || arguments.callee.caller.arguments[0];
      if (e && e.keyCode === 17) {
        // 按 Esc
        that.ctrlDown = true;
      }
    };
    document.onkeyup = function(event) {
      const e = event || window.event || arguments.callee.caller.arguments[0];
      if (e && e.keyCode === 17) {
        // 按 Esc
        that.ctrlDown = false;
      }
    };
  }

  ngOnChanges(value) {
    console.log(value);
  }
  public ngOnInit() {
    console.log(this.Xmpp.column.columnNames);
    console.log(this.Xmpp.task.showTask);
  }

  test(c: XmppColumn, task: XmppTask, $event?: any) {
    const keyCode = $event.which;
    const activeCodes = [13, 40, 38, 37, 39];

    if (activeCodes.indexOf(keyCode) !== -1) {
      $event.preventDefault();
      let nextTask;
      switch ($event.which) {
        case 13:
          // 回车
          nextTask = this.Xmpp.allTasks[this.currentTask.id];
          if (!nextTask) {
            return;
          }
          this.currentTask = this.deepClone(nextTask);
          break;
        case 40:
          // 向下
          // this.currentId = this.currentId + 1;
          nextTask = this.Xmpp.allTasks[this.currentTask.id];
          if (!nextTask) {
            return;
          }
          this.currentTask = this.deepClone(nextTask);
          break;
        case 38:
          // 向上
          nextTask = this.Xmpp.allTasks[this.currentTask.id - 2];
          if (!nextTask) {
            return;
          }
          this.currentTask = this.deepClone(nextTask);
          break;

        default:
          break;
      }
      if ($event.which === 39 || $event.which === 37) {
        console.log(this.Xmpp.column.columnNames);
        const findIndex = this.Xmpp.column.columnNames.findIndex(
          (column) => column.key === this.currentKey
        );
        if (
          !findIndex ||
          findIndex === this.Xmpp.column.columnNames.length - 1
        ) {
          return;
        }
        let key;
        if ($event.which === 37) {
          key = this.Xmpp.column.columnNames[findIndex - 1].key;
        }
        if ($event.which === 39) {
          key = this.Xmpp.column.columnNames[findIndex + 1].key;
        }
        this.currentKey = key;
      }
      setTimeout(() => {
        this.focusAndMoveToEnd(
          document
            .querySelector('div.column.active')
            .getElementsByTagName('input')[0]
        );
        // this.moveEnd(document.querySelector('div.column.active').getElementsByTagName('input')[0])
      }, 100);
    }
  }

  // moveEnd(obj) {
  //   obj.focus();
  //   var len = obj.value.length;
  //   if (window.getSelection()) {
  //     var sel = obj.createTextRange();
  //     sel.moveStart('character', len);
  //     sel.collapse();
  //     sel.select();
  //   } else if (typeof obj.selectionStart == 'number' && typeof obj.selectionEnd == 'number') {
  //     obj.selectionStart = obj.selectionEnd = len;
  //   }
  // }

  // changeDate(c: XmppColumn, task, $event) {
  //   console.log($event.target.value)
  //   let value = $event.target.value;
  //   if (isNaN(value) && !isNaN(Date.parse(value))) {
  //     task[c.key] = value;
  //     this.Xmpp.render()
  //   }
  //   $event.target.blur()
  // }

  cloneTask(c: XmppColumn, task: XmppTask) {
    this.currentTask = this.deepClone(task);
    this.currentKey = c.key;
  }

  editStuteChange(
    c: XmppColumn,
    task: XmppTask,
    $event?: any,
    needRender = false
  ) {
    const value = $event ? $event.target.value : null;
    if (value === task[c.key]) {
      return;
    }

    // 日期
    if (c.type === 'date') {
      if (task[c.key] === value) {
        return;
      }
      if (isNaN(value) && !isNaN(Date.parse(value))) {
        if (c.key === 'startDate') {
          this.Xmpp.task.setStartDate(task, value);
        }

        if (c.key === 'endDate') {
          this.Xmpp.task.setEndDate(task, value);
        }

        if (c.key === 'actualStartDate') {
          task.actualStartDate = value;
        }

        if (c.key === 'actualEndDate') {
          task.actualEndDate = value;
        }
      }
    }

    if (c.type === 'input') {
      // 前置任务
      if (c.key === 'prevRelationStr') {
        const prevRelations = [];
        const str = value.replace(new RegExp('，', 'gm'), ',').toUpperCase();
        if (str === task.prevRelationStr) {
          return;
        }
        const relationArray = str.split(',');
        let delay = 0;
        for (const relationStr of relationArray) {
          const prevId = parseInt(relationStr);
          const finder = prevRelations.find((item) => item.prevId === prevId);
          if (finder) {
            continue;
          }
          let type;
          if (relationStr.indexOf('FF') !== -1) {
            type = PREVTYPE.FF;
          }
          if (relationStr.indexOf('SF') !== -1) {
            type = PREVTYPE.SF;
          }
          if (relationStr.indexOf('SS') !== -1) {
            type = PREVTYPE.SS;
          }
          if (relationStr.indexOf('FS') !== -1) {
            type = PREVTYPE.FS;
          }
          if (prevId === relationStr) {
            type = PREVTYPE.FS;
          }
          if (relationStr.indexOf('+') !== -1) {
            const delayStr = relationStr.split('+')[1];
            if (!isNaN(parseInt(delayStr))) {
              delay = parseInt(delayStr);
            }
          }
          if (prevId && type) {
            // 此处修改/添加的preRelation没有id，该任务需要保存link
            prevRelations.push(
              new XmppPredecessorLink({
                prevId,
                relation: type,
                delay,
                isDelete: false,
              })
            );
          }
        }
        const checkResult = this.checkRelations(task, prevRelations);
        if (checkResult) {
          this.message.error(checkResult);
          // alert(checkResult);
        } else {
          task.prevRelation = prevRelations;
          $event.target.value = task.prevRelationStr;
        }
        needRender = true;
      }

      // 资源
      if (c.key === 'assignmentsStr') {
        this.updateAssginmentStr(task, value);
        needRender = true;
      }

      // 工期
      if (c.key === 'duration' || c.key === 'actualDuration') {
        this.Xmpp.task.setDuration(task, value);
        needRender = true;
      }

      // 完成率
      if (c.key === 'finishRate') {
        task.finishRate = value;
      }
      // 任务名
      if (c.key === 'taskName') {
        task.taskName = value;
      }
      // 任务等级
      if (c.key === 'taskLevel') {
        task.taskLevel = value;
      }
      // 是否里程碑
      if (c.key === 'isMilepost') {
        task.isMilepost = value;
      }
    }

    if (needRender) {
      this.Xmpp.render();
    }

    // setTimeout(() => {
    //   // console.log(this.editInput)
    //   if (this.editInput) {
    //     this.editInput.nativeElement.focus();
    //   }
    // }, 300);
  }

  updateAssginmentStr(task, value: string): XmppAssignment {
    if (!value) {
      return;
    }
    const str = value.replace(new RegExp('，', 'gm'), ',');
    if (str === task.assignmentsStr) {
      return;
    }

    const assignmentsStrArray = str.split(',');
    const newAssignments: XmppAssignment[] = [];
    for (const resourceName of assignmentsStrArray) {
      // let finder = task.assignments.find(as => as.resourceName == resourceName);
      const resource = this.Xmpp.mpp.mppInfo.resources.find(
        (rs) => rs.name === resourceName
      );
      const maxResourceUid =
        this.Xmpp.mpp.findMaxUid(this.Xmpp.mpp.mppInfo.resources) + 1;
      if (!resource) {
        this.Xmpp.mpp.mppInfo.resources.push({
          uid: maxResourceUid,
          name: resourceName,
          start: task.startDate,
          finish: task.endDate,
          parentId: this.Xmpp.mpp.mppInfo.id,
        });
      }
      newAssignments.push({
        resourceUID: resource ? resource.uid : maxResourceUid,
        start: task.startDate,
        finish: task.endDate,
        parentId: this.Xmpp.mpp.mppInfo.id,
        resourceName,
        taskUID: task.uid,
      });
    }

    task.assignments = newAssignments;
  }

  resizeCallback($event, column) {
    const finder = this.Xmpp.column.columnNames.find(
      (item) => item.key === column.key
    );
    if (finder) {
      finder.width += $event.x;
      this.Xmpp.column.totalWidth += $event.x;
    }
  }

  // addTaskFromNewLine(type: string, $event) {

  //   if (!$event || $event === '') {
  //     return;
  //   }

  //   this.Xmpp.task.activeTaskId = null;
  //   if (type === 'name' && $event !== '') {
  //     this.Xmpp.task.addTaskHandle({ taskName: $event });
  //   }
  //   if (type === 'startDate') {
  //     this.Xmpp.task.addTaskHandle({ startDate: $event, duration: 1 });
  //   }
  //   if (type === 'endDate') {
  //     this.Xmpp.task.addTaskHandle({ endDate: $event, duration: 1 });
  //   }

  //   this.newTask = {
  //     id: -1,
  //     taskName: '',
  //     startDate: null,
  //     endDate: null,
  //   };

  //   setTimeout(() => {
  //     // this.newTask = null;
  //     this.newTaskBlur.emit();
  //     setTimeout(() => {
  //       if (this.editInput) {
  //         this.editInput.nativeElement.focus();
  //       }
  //     }, 500);
  //   }, 500);
  // }

  // 工期blur

  /*
  工作安排中， 禁用计划完成时间早于当前时间的任务
  */
  // public isBeforeCurrent(endDate: any) {
  //   const isPlanning = false;
  //   if (isPlanning) {
  //     const currentDate = this.currentDate;
  //     const isBefore = moment(endDate).isBefore(currentDate);
  //     if (isBefore) {
  //       return true;
  //     } else {
  //       return false;
  //     }
  //   } else {
  //     return false;
  //   }
  // }

  /*
   * 时间选择器
   */
  public dateClickTask(task: any) {
    this.updateServiceData();
  }

  private updateServiceData() {
    this.Xmpp.render();
  }

  public createNotification = (type, title, message) => {
    this._notification.create(type, title, message);
  }
  public foldTaskLevel(task) {
    const index = task.id - 1;

    const allTasks = this.Xmpp.allTasks;
    const children = allTasks[index].childTaskID;

    for (let i = 0; i < children.length; i++) {
      const child = children[i] - 1;
      allTasks[child].isSelected = false;
    }

    allTasks[index].isFold = true;
    this.Xmpp.render();
  }

  public openTaskLevel(task) {
    const index = task.id - 1;
    this.Xmpp.allTasks[index].isFold = false;
    this.Xmpp.render();
  }

  // 全选
  public selectAllTask() {
    const allTasks = this.Xmpp.allTasks;
    const allSelected = this.isAllselected;
    if (allSelected) {
      allTasks.forEach((element) => {
        element.isSelected = true;
      });
      this.Xmpp.task.selectedTasks = this.Xmpp.allTasks;
    } else {
      allTasks.forEach((element) => {
        element.isSelected = false;
      });
      this.Xmpp.task.selectedTasks = [];
    }
    // console.log(this.Xmpp.task.selectedTasks);
    // this.Xmpp.render();
  }

  selectAll() {
    this.Xmpp.task.isAllSelect = !this.Xmpp.task.isAllSelect;
    if (this.Xmpp.task.isAllSelect) {
      this.Xmpp.task.selectedTasks = this.Xmpp.allTasks;
    } else {
      this.Xmpp.task.selectedTasks = [];
    }
    this.Xmpp.allTasks.forEach((task) => {
      task.isSelected = this.Xmpp.task.isAllSelect;
    });
    // console.log(this.Xmpp.task.selectedTasks);
  }

  clickTaskRow($event, task: XmppTask) {
    // if (!this.Xmpp.task.activeTaskId || (this.Xmpp.task.activeTaskId && this.Xmpp.task.activeTaskId !== task.id)) {
    //   this.Xmpp.task.activeTaskId = task.id;
    //   task.isSelected = true;
    // }

    clearTimeout(this.clickTimer);
    const that = this;
    this.clickTimer = setTimeout(function() {
      // 在单击事件中添加一个setTimeout()函数，设置单击事件触发的时间间隔
      that.Xmpp.task.taskHandleListener.next({
        event: $event,
        task,
        XmppInfo: that.Xmpp,
      });
      if (that.ctrlDown) {
        task.isSelected = !task.isSelected;
        const index = that.Xmpp.task.selectedTasks.findIndex(
          (item) => item.id === task.id
        );
        if (task.isSelected) {
          if (index === -1) {
            that.Xmpp.task.selectedTasks.push(task);
          }
        } else {
          if (index !== -1) {
            that.Xmpp.task.selectedTasks.splice(index, 1);
          }
        }
      } else {
        that.Xmpp.task.selectedTasks.forEach((tk) => {
          if (tk.id !== task.id) {
            tk.isSelected = false;
          }
        });
        if (!task.isSelected) {
          task.isSelected = true;
          that.Xmpp.task.selectedTasks = [task];
        } else {
          task.isSelected = false;
          that.Xmpp.task.selectedTasks = [];
          that.Xmpp.draw.selectedTaskId = null;
          // that.Xmpp.draw.ctxMask.clearRect(0, 0, that.Xmpp.draw.canvasWidth * that.Xmpp.draw.ctxRatio, that.Xmpp.draw.canvasHeight * that.Xmpp.draw.ctxRatio);
        }
      }
      if (that.Xmpp.task.selectedTasks.length === 0) {
        that.Xmpp.draw.ctxMask.clearRect(
          0,
          0,
          that.Xmpp.draw.canvasWidth * that.Xmpp.draw.ctxRatio,
          that.Xmpp.draw.canvasHeight * that.Xmpp.draw.ctxRatio
        );
      } else {
        if (task.id !== -1) {
          that.Xmpp.draw.ctxMask.clearRect(
            0,
            0,
            that.Xmpp.draw.canvasWidth * that.Xmpp.draw.ctxRatio,
            that.Xmpp.draw.canvasHeight * that.Xmpp.draw.ctxRatio
          );
          that.Xmpp.draw.drawSelectTask(task.id);
        }
      }
    }, 100);

    // setTimeout(() => {
    //   this.focusAndMoveToEnd(document.querySelector('div.column.active').getElementsByTagName('input')[0])
    // }, 100);
  }

  focusAndMoveToEnd(ele) {
    ele.focus();
    ele.selectionEnd = ele.value.length;
    const len = ele.value.length;
    ele.selectionStart = ele.selectionEnd = len;
  }

  deepClone(task: XmppTask) {
    const cloneTask = {
      id: task.id,
      startDate: task.startDate,
      endDate: task.endDate,
      duration: task.duration,
      actualStartDate: task.actualStartDate,
      actualEndDate: task.actualEndDate,
      actualDuration: task.actualDuration,
      taskName: task.taskName,
      prevRelationStr: task.prevRelationStr,
    };
    return JSON.parse(JSON.stringify(cloneTask));
  }

  /**
   * 双击显示设置弹窗
   * @param task
   */
  public showSettings($event, task: XmppTask) {
    clearTimeout(this.clickTimer);
    this.Xmpp.task.taskHandleListener.next({
      event: $event,
      task,
      XmppInfo: this.Xmpp,
    });
    const relations = [];
    task.prevRelation.forEach((ele) => {
      relations.push(ele);
    });
    if (relations.length === 0) {
      relations.push(
        new XmppPredecessorLink({
          relation: 1,
          delay: 0,
        })
      );
    }
    console.log(task);
    const assignmentsStr = task.assignmentsStr;
    this.editInfo = new EditModel({
      relations,
      taskName: task.taskName,
      assignmentsStr,
      taskLevel: task.taskLevel,
      isMilepost: task.isMilepost,
    });
    console.log(this.editInfo);
    this.settingTask = task;
    this.settingModalView = true;
  }

  /**
   * 设置弹窗提交事件
   * @param xmpp
   */
  public settingSave() {
    const relations = this.editInfo.relations;
    const prevRelation = [];
    relations.forEach((element, index) => {
      const finder = prevRelation.find(
        (item) => item.prevId === element.prevId
      );
      if (element.prevId && !isNullOrUndefined(element.relation) && !finder) {
        prevRelation.push(
          new XmppPredecessorLink({
            id: element.id,
            prevId: parseInt(element.prevId),
            relation: element.relation,
            delay: element.delay,
            isDelete: element.isDelete,
          })
        );
      }
    });
    const checkResult = this.checkRelations(this.settingTask, prevRelation);
    if (checkResult) {
      this.message.error(checkResult);
      // alert(checkResult);
    } else {
      this.settingTask.prevRelation = prevRelation;
      this.settingTask.taskName = this.editInfo.taskName;
      this.settingTask.taskLevel = this.editInfo.taskLevel;
      this.settingTask.isMilepost = this.editInfo.isMilepost;
      this.updateAssginmentStr(this.settingTask, this.editInfo.assignmentsStr);
      this.Xmpp.render();
      this.settingModalView = false;
    }
  }

  public checkRelations(task, relations: any[]) {
    let checkRepeat: string;
    let checkSameId: string;
    let checkLoop: string;
    let checkPrevId: string;
    const allTasks = this.Xmpp.allTasks;
    const taskLength = allTasks.length;
    const usefulRelation = [];
    relations.forEach((rela) => {
      if (!rela.isDelete) {
        usefulRelation.push(rela);
      }
    });
    // 检查4: 非法字符
    usefulRelation.forEach((element) => {
      const id = parseInt(element.prevId);
      if (isNaN(id)) {
        checkPrevId = `任务标识必须为数字`;
        return false;
      } else {
        if (id < 1 || id > taskLength) {
          checkPrevId = `任务标识不存在, error:'${id}'`;
          return false;
        }
      }
    });

    // 检查1： 前置任务id重复
    usefulRelation.forEach((i) => {
      const id = i.prevId;
      const idArray = [];
      usefulRelation.forEach((j) => {
        if (j.prevId === id) {
          idArray.push(j);
        }
      });
      if (idArray.length > 1) {
        checkRepeat = `任务'${idArray[0]}'两次链接到同一个后续任务`;
        return;
      }
    });

    // 检查2：prevId与任务Id相同
    usefulRelation.forEach((element) => {
      const id = element.prevId;
      if (id === task.id) {
        checkSameId = `前置任务不能为自己, error:${id}`;
        return;
      }
    });

    // 检查3: 是否产生循环
    usefulRelation.forEach((element) => {
      const id = element.prevId;
      const prevTask = allTasks[id - 1];
      let check;
      if (prevTask && prevTask.prevRelation.length > 0) {
        check = prevTask.prevRelation.findIndex((relation) => {
          return relation.prevId === task.id;
        });
        if (check !== -1) {
          checkLoop = `任务'${id}'产生循环`;
          return;
        }
      }
    });

    if (checkPrevId) {
      return checkPrevId;
    } else if (checkRepeat) {
      return checkRepeat;
    } else if (checkSameId) {
      return checkSameId;
    } else if (checkLoop) {
      return checkLoop;
    } else {
      return false;
    }
  }

  public addRelation() {
    this.editInfo.relations.push(
      new XmppPredecessorLink({
        relation: 1,
        delay: 0,
      })
    );
  }

  public deleteRelation(index: number) {
    this.editInfo.relations.splice(index, 1);
    // this.editInfo.relations[index].isDelete = 1;
  }

  // public clickCheckBox(task) {
  //   this.Xmpp.task.caculateListener.next();
  // }

  public submitFinishBinds() {
    // this.ganttHelpServ.saveTasksHanle(this.Xmpp, 'save');
  }

  // 里程碑回调
  onIsMilepostChange(event: boolean) {
    // console.log(event);
    if (event) {
      console.log(this.Xmpp.task.selectedTasks);
      this.Xmpp.task.selectedTasks.forEach((ts) => {
        ts.duration = 0;
        ts.isMilepost = true;
      });
      // console.log(this.Xmpp.allTasks);
      // this.Xmpp.render();
    } else {
      console.log(this.Xmpp.task.selectedTasks);
      this.Xmpp.task.selectedTasks.forEach((ts) => {
        ts.duration = 0;
        ts.isMilepost = false;
      });
      // this.Xmpp.render();
    }
  }
}
