import { Injectable } from '@angular/core';
import moment from 'moment';
import { NzMessageService } from 'ng-zorro-antd';
import { GanttRequestService } from './gantt-request.service';
import { PREVTYPE, Xmpp, XmppTask } from '../src/api-public';
import { isNullOrUndefined } from 'util';
@Injectable({
  providedIn: 'root'
})
export class GanttHelperService {
  public settingVisible = false;
  public currentTask: XmppTask;
  // relations: any = [];
  // alreadyDeleteTasks: any[] = [];
  loading = false;
  public constructor(
    private message: NzMessageService,
    private ganttRequestSev: GanttRequestService
  ) {

  }


  // public async saveTasksHanle(xmpp: Xmpp, type: string) {
  //   const getGuid = () => {
  //     const s = [];
  //     const hexDigits = '0123456789abcdef';
  //     for (let i = 0; i < 36; i++) {
  //       s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
  //     }
  //     s[14] = '4'; // bits 12-15 of the time_hi_and_version field to 0010
  //     s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
  //     s[8] = s[13] = s[18] = s[23] = '-';
  //     const uuid = s.join('');
  //     return uuid;
  //   };
  //   const allTasks = xmpp.allTasks;
  //   const taskHasSqlId = [];
  //   const alreadyAddTasks: XmppTask[] = [];
  //   const alreadyEditTasks: XmppTask[] = [];
  //   const alreadyDeleteTasks: XmppTask[] = this.alreadyDeleteTasks;
  //   this.loading = true;

  //   xmpp.mpp.ParentId2WBS();

  //   // 组装 alreadyEditTasks
  //   for (const task of allTasks) {
  //     if (task.defaultData) {
  //       taskHasSqlId.push(task);
  //     } else {
  //       xmpp.task.maxUID = xmpp.task.maxUID + 1;
  //       task.uid = xmpp.task.maxUID;
  //       alreadyAddTasks.push(task);
  //     }
  //   }


  //   // 组装 alreadyEditTasks
  //   for (const element of taskHasSqlId) {
  //     const defaultData = element.defaultData;
  //     const dateKey = ['startDate', 'endDate', 'actualStartDate', 'actualEndDate'];
  //     const arrayKey = ['bindings', 'childTaskID', 'prevRelation'];
  //     for (const itemKey in defaultData) {
  //       // 工期变化不考虑，再次获取tasklist会重新计算工期
  //       if (itemKey === 'duration' || itemKey === 'actualDuration') {
  //         continue;
  //       }
  //       // 时间参数变化
  //       if (dateKey.indexOf(itemKey) !== -1) {
  //         const elementDate = moment(element[itemKey]).format('YYYY MM DD');
  //         const defaultDate = moment(defaultData[itemKey]).format('YYYY MM DD');
  //         if (elementDate !== defaultDate) {
  //           const datefinder = alreadyEditTasks.findIndex((task) => {
  //             return task.sqId === element.sqId;
  //           });
  //           if (datefinder === -1) {
  //             alreadyEditTasks.push(element);
  //           }
  //         }
  //         continue;
  //       }
  //       // 数组类型参数变化
  //       if (arrayKey.indexOf(itemKey) !== -1) {
  //         const elementArray = element[itemKey].toString();
  //         const defaultArray = defaultData[itemKey].toString();
  //         if (elementArray !== defaultArray) {
  //           const arrayfinder = alreadyEditTasks.findIndex((task) => {
  //             return task.sqId === element.sqId;
  //           });
  //           if (arrayfinder === -1) {
  //             alreadyEditTasks.push(element);
  //           }
  //         }
  //         continue;
  //       }

  //       // 普通参数变化
  //       if (element[itemKey] !== defaultData[itemKey]) {
  //         const normalfinder = alreadyEditTasks.findIndex((task) => {
  //           return task.sqId === element.sqId;
  //         });
  //         if (normalfinder === -1) {
  //           alreadyEditTasks.push(element);
  //         }
  //       }

  //     }
  //   }

  //   // 查询id是否连续
  //   const finder = allTasks.find((task, index) => {
  //     return task.id !== index + 1;
  //   });

  //   if (finder) {
  //     // 有错乱
  //     console.warn(finder);
  //     this.message.error('orderId重复');
  //   } else {
  //     const tasks = alreadyAddTasks.concat(alreadyEditTasks);
  //     // 整理
  //     const paramJson = [];
  //     alreadyAddTasks.forEach((task) => {
  //       task.sqId = getGuid();
  //       paramJson.push({
  //         op: 'create',
  //         type: 'task',
  //         data: task.toMpp()
  //       });
  //     });
  //     alreadyEditTasks.forEach((task) => {
  //       paramJson.push({
  //         op: 'update',
  //         type: 'task',
  //         data: task.toMpp()
  //       });
  //     });
  //     alreadyDeleteTasks.forEach((sqid) => {
  //       paramJson.push({
  //         op: 'delete',
  //         type: 'task',
  //         data: { Id: sqid }
  //       });
  //     });
  //     console.log(paramJson);
  //     this.dealwithPredecessorLink(xmpp, tasks, paramJson);
  //     const res = await this.ganttRequestSev.putTasks(xmpp.mpp.mppInfo.id, paramJson);
  //     if (res) {
  //       this.message.success('提交成功');
  //     } else {
  //       this.message.success('提交成功');
  //     }
  //     this.loading = false;
  //   }
  // }

  dealwithPredecessorLink(xmpp: Xmpp, tasks: XmppTask[], paramJson: any[]) {
    for (const element of tasks) {
      const relation = element.prevRelation;
      relation.forEach((prev) => {
        // 处理PredecessorLink
        if (prev.id && !prev.isDelete) {
          paramJson.push({
            op: 'update',
            type: 'link',
            data: {
              PredecessorUID: xmpp.allTasks[prev.prevId - 1].uid,
              ParentId: element.sqId,
              Type: prev.relation,
              LinkLag: prev.delay * 8 * 600,
              Id: prev.id
            }
          });
        } else if (prev.id && prev.isDelete) {
          paramJson.push({
            op: 'delete',
            type: 'link',
            data: {
              Id: prev.id
            }
          });
        } else if (!prev.id && !prev.isDelete) {
          paramJson.push({
            op: 'create',
            type: 'link',
            data: {
              PredecessorUID: xmpp.allTasks[prev.prevId - 1].uid,
              ParentId: element.sqId,
              Type: prev.relation,
              LinkLag: prev.delay ? prev.delay * 8 * 600 : 0
            }
          });
        }
      });
    }
  }



  public showRelation(task) {
    this.currentTask = task;
    const showRelation = [];
    if (task.prevRelation.length > 0) {
      task.prevRelation.forEach((item) => {
        const relation = this.translateNumber(item.relation);
        const delay = item.delay ? `+${item.delay}` : '';
        if (item.prevId && !item.isDelete) {
          const string = `${item.prevId}${relation}${delay}`;
          showRelation.push(string);
        }
      });
    }
    if (showRelation.length === 0) {
      return '请选择';
    }
    return showRelation.join(',');
  }

  public translateNumber(relation) {
    if (relation === PREVTYPE.FS) {
      return 'FS';
    }
    if (relation === PREVTYPE.SS) {
      return 'SS';
    }
    if (relation === PREVTYPE.FF) {
      return 'FF';
    }
    if (relation === PREVTYPE.SF) {
      return 'SF';
    }
  }


}
