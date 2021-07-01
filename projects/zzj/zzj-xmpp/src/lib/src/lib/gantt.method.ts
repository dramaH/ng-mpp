import moment, { weekdays } from 'moment';
import {
  IXmpp,
  IMPPProject,
  IMppTask,
  XmppTask,
  XmppPredecessorLink,
  XmppWeekDayType,
  XmppWeekDay,
  XmppResource,
  XmppAssignment,
} from './gantt-interface';
import { PREVTYPE, EXTENDATTRS } from './gantt.config';
import { Moment } from 'moment';
import { isArray } from 'util';

export class GanttMethod {
  static mpp = {
    dealWithProject(Gantt: IXmpp, ganttInfo: IMPPProject) {
      Gantt.mpp.mppInfo = ganttInfo;
      // 处理例外日期
      Gantt.calendar.exceptDate = [];
      for (const calendar of ganttInfo.calendars) {
        if (calendar.exceptions.length > 0) {
          for (const except of calendar.exceptions) {
            Gantt.calendar.exceptDate.push({
              name: except.name,
              fromDate: moment(except.fromDate).format('YYYY-MM-DD'),
              toDate: moment(except.toDate).format('YYYY-MM-DD'),
              id: except.id,
              parentId: except.parentId,
            });
          }
        }
      }

      // 处理工作周， 每周休假的日期：周日：1，周一：2
      // const pauseWeekDayTypes = [];
      const weekDays = [];
      let dayType = ganttInfo.weekStartDay + 1;
      if (ganttInfo.calendars.length > 0) {
        for (const calendar of ganttInfo.calendars) {
          // 查找standard
          if (calendar.weekDays.length > 0 && calendar.uid === 1) {
            // weekDays = calendar.weekDays;
            Gantt.calendar.calendarId = calendar.id;

            for (let i = 0; i < 7; i++) {
              const fidner = calendar.weekDays.find(
                (wd) => wd.dayType === dayType
              );
              if (fidner) {
                weekDays.push({
                  dayType: fidner.dayType,
                  dayText: XmppWeekDayType[fidner.dayType],
                  dayWorking: fidner.dayWorking,
                  id: fidner.id,
                  fromDate: fidner.fromDate,
                  toDate: fidner.toDate,
                });
              } else {
                weekDays.push({
                  dayType,
                  dayText: XmppWeekDayType[dayType],
                  dayWorking: true,
                });
              }
              if (dayType === 7) {
                dayType = 1;
              } else {
                dayType++;
              }
            }
          }
        }
      }
      // Gantt.calendar.pauseWeekDayTypes = pauseWeekDayTypes;
      Gantt.calendar.weekDays = weekDays;
    },
    dealWithMPPTasks(Gantt: IXmpp, mppTasks: IMppTask[]) {
      Gantt.task.isFirstInit = true;
      const allTasks = [];
      const maxUID = 0;
      const assignments = Gantt.mpp.mppInfo.assignments;
      const resources = Gantt.mpp.mppInfo.resources;
      const resourcesMap: Map<number, XmppResource> = new Map<
        number,
        XmppResource
      >();
      resources.forEach((rs) => {
        if (rs.name) {
          resourcesMap.set(rs.uid, rs);
        }
      });
      const findCustomExtendAttr = (ExtendedAttribute) => {
        const finder = ExtendedAttribute.find((attr) => {
          return attr.fieldID === EXTENDATTRS.binding.FieldID;
        });
        if (finder) {
          return finder;
        } else {
          return null;
        }
      };

      const findAssignments = (uid) => {
        const taskAssignments: XmppAssignment[] = [];
        for (const ag of assignments) {
          if (ag.taskUID === uid) {
            const resource: XmppResource = resourcesMap.get(ag.resourceUID);
            if (resource) {
              const find = taskAssignments.find((taskRs) => {
                return taskRs.resourceName === resource.name;
              });
              if (!find) {
                taskAssignments.push({
                  id: ag.id,
                  resourceUID: resource.uid,
                  uid: ag.uid,
                  resourceName: resource.name,
                  start: ag.start,
                  finish: ag.finish,
                  parentId: ag.parentId,
                  taskUID: ag.taskUID,
                });
              }
            }
          }
        }
        return taskAssignments;
        // return assignments.find(ag => {
        //     return ag.taskUID == uid;
        // })
      };

      mppTasks.forEach((element, index) => {
        if (element._ID) {
          const symbol = index + 1000;
          let customAttrs: any = {};
          if (element.customAttrs) {
            customAttrs = JSON.parse(element.customAttrs);
          }
          const defaultData = {
            Xmpp: Gantt,
            sqId: element.id,
            // exceptDate: Gantt.calendar.exceptDate ? Gantt.calendar.exceptDate : [],
            uid: element.uid,
            id: element._ID,
            symbol,
            wbs: element.wbs,
            taskName: element.name ? element.name : '',
            duration: GanttMethod.mpp.PT2Duration(element.duration),
            startDate: element.start
              ? GanttMethod.date.dateDeepClone(element.start)
              : null,
            endDate: element.finish
              ? GanttMethod.date.dateDeepClone(element.finish)
              : null,
            actualStartDate: element.actualStart
              ? GanttMethod.date.dateDeepClone(element.actualStart)
              : null,
            actualDuration: GanttMethod.mpp.PT2Duration(element.actualDuration),
            actualEndDate: element.actualFinish
              ? GanttMethod.date.dateDeepClone(element.actualFinish)
              : null,
            childTaskID: GanttMethod.mpp.WBS2ParentId(element, mppTasks)
              .childId,
            parentTaskID: GanttMethod.mpp.WBS2ParentId(element, mppTasks)
              .parentId,
            level: GanttMethod.mpp.WBS2ParentId(element, mppTasks).level,
            prevRelation: GanttMethod.predecessorLink.dealWithPrev(
              element,
              mppTasks
            ),
            extendedAttribute: element.extendedAttribute,
            customExtendAttr: findCustomExtendAttr(element.extendedAttribute),
            ganttChartId: element.parentId,
            tags: customAttrs.tags,
            finishRate: customAttrs.finishRate,
            taskLevel: customAttrs.taskLevel,
            isMilepost: element.isMilepost,
            assignments: findAssignments(element.uid),
          };

          const task = new XmppTask(defaultData);

          // if (bindings && bindings.length > 0) {
          //     let extendAttrs = Gantt.mpp.extraAttrMap.get(symbol);
          //     if (extendAttrs) {
          //         extendAttrs.bindings = bindings;
          //         Gantt.mpp.extraAttrMap.set(symbol, extendAttrs);
          //     } else {
          //         Gantt.mpp.extraAttrMap.set(symbol, {bindings});
          //     }
          // }
          allTasks.push(task);
        }
        if (element.uid > maxUID) {
          Gantt.task.maxUID = element.uid;
        }
      });
      Gantt.globalLoading = false;
      Gantt.task.isFirstInit = false;
      return allTasks;
    },

    PT2Duration(str) {
      if (!str) {
        return 0;
      }
      const number = str.match(/PT(\S*)H/)[1];
      if (isNaN(number)) {
        return 0;
      } else {
        return Number(number) / 8;
      }
    },

    WBS2ParentId(task: IMppTask, allTasks: IMppTask[]) {
      let level = 1;
      const findParentId = (ptask: IMppTask) => {
        if (ptask.wbs) {
          const array = ptask.wbs.split('.');
          level = array.length;
          if (array.length === 1) {
            return null;
          }
          array.pop();
          const newStr = array.join('.');
          const finder = allTasks.find((element) => {
            return element.wbs === newStr;
          });
          if (finder) {
            return finder._ID;
          } else {
            return null;
          }
        }
        return null;
      };

      const childId = [];
      const findChildId = (ctask: IMppTask) => {
        for (
          let i = parseInt(ctask._ID.toString(), 0);
          i < allTasks.length;
          i++
        ) {
          const element = allTasks[i];
          if (element.wbs) {
            const array = element.wbs.split('.');
            array.pop();
            const newStr = array.join('.');
            if (newStr === ctask.wbs) {
              childId.push(element._ID);
            }
          }
        }
      };
      findChildId(task);
      return {
        parentId: findParentId(task),
        childId,
        level,
      };
    },

    ParentId2WBS(Gantt) {
      const loopChild = (allTasks: any, level: number) => {
        let index = 1;
        for (const element of allTasks) {
          if (element.level === level) {
            const array = new Array();
            const parentTaskWBS = element.parentTaskID
              ? Gantt.allTasks[element.parentTaskID - 1].wbs
              : [];
            parentTaskWBS.forEach((wbs) => {
              array.push(wbs);
            });
            array.push(index.toString());
            element.wbs = array;
            index++;
            if (element.childTaskID.length > 0) {
              const childTask = [];
              element.childTaskID.forEach((chidId) => {
                childTask.push(Gantt.allTasks[chidId - 1]);
              });
              loopChild(childTask, level + 1);
            }
          }
        }
      };
      loopChild(Gantt.allTasks, 1);
      Gantt.allTasks.forEach((ele) => {
        if (isArray(ele.wbs)) {
          ele.wbs = ele.wbs.join('.');
        }
      });
    },

    findMaxUid(tragetArray: any): number {
      let maxUID = 1;
      for (const item of tragetArray) {
        if (item.uid && item.uid > maxUID) {
          maxUID = item.uid;
        }
      }
      return maxUID;
    },
  };
  static canvas = {
    updateCanvasInfo(Gantt: IXmpp) {
      const allTasks = Gantt.allTasks;
      const calenderArry = [];
      if (Gantt.allTasks.length === 0) {
        Gantt.calendar.calenderWidth = 0;
        Gantt.draw.canvasInfo = [];
        Gantt.calendar.weeksArry = [];
        return;
      }

      let lastFinishTask = 0;
      let lastActualFinishTask = 0;
      let lastFinishDay = allTasks[0].endDate;
      let lastActualFinishDay = allTasks[0].actualEndDate;
      const baseWidth = Gantt.calendar.baseCellWidth;
      for (const task of allTasks) {
        const endDate = task.endDate;
        const startDate = task.startDate;
        const actualEndDate = task.actualEndDate;
        const actualStartDate = task.actualStartDate;
        if (endDate && startDate) {
          calenderArry.push(moment(task.startDate));
          calenderArry.push(moment(task.endDate));
          if (
            moment(endDate).clone().diff(moment(lastFinishDay), 'days') >= 0
          ) {
            // 找到日期最后一天对应的任务id
            lastFinishTask = task.id;
            lastFinishDay = task.endDate;
          }
        }
        if (actualEndDate && actualStartDate) {
          calenderArry.push(moment(task.actualStartDate));
          calenderArry.push(moment(task.actualEndDate));
          if (!lastActualFinishDay) {
            lastActualFinishTask = task.id;
            lastActualFinishDay = actualEndDate;
          } else {
            if (
              moment(actualEndDate)
                .clone()
                .diff(moment(lastActualFinishDay), 'days') >= 0
            ) {
              // 找到日期最后一天对应的任务id
              lastActualFinishTask = task.id;
              lastActualFinishDay = task.actualEndDate;
            }
          }
        }
      }
      // showTask最早开始/最晚结束日期
      const minDay = moment.min(calenderArry);
      const maxDay = moment.max(calenderArry);
      // 项目日历设置：自然周从周几开始 0：周日，1：周一
      // const weekStartDay = Gantt.mpp.mppInfo.weekStartDay;
      let weekStartDay = 1;
      if (Gantt.calendar.weekDays.length > 0) {
        weekStartDay = Number(Gantt.calendar.weekDays[0].dayType) - 1;
      }

      // 最早/最晚日期对应的周几
      const minWeekDay: number = new Date(minDay.unix() * 1000).getDay();
      const maxWeekDay: number = new Date(maxDay.unix() * 1000).getDay();
      // 日历开始/结束日期
      const oneDayLong = 24 * 60 * 60;
      let minDayDiffer = minWeekDay - weekStartDay;
      if (minDayDiffer < 0) {
        minDayDiffer = 7 + minDayDiffer;
      }
      let maxDayDiffer = maxWeekDay - weekStartDay;
      if (maxDayDiffer < 0) {
        maxDayDiffer = 7 + maxDayDiffer;
      }
      let minLineDay = moment();
      let maxLineDay = moment();
      minLineDay = moment.unix(minDay.unix() - minDayDiffer * oneDayLong);
      maxLineDay = moment.unix(maxDay.unix() - maxDayDiffer * oneDayLong);
      Gantt.calendar.minLineDay = minLineDay;
      Gantt.calendar.maxLineDay = minLineDay;
      // 计算每周开始那天 数组

      const weekCount =
        Math.ceil(maxLineDay.clone().diff(minLineDay.clone(), 'days') / 7) + 1;
      const weeksArry = [];
      const weekExceptDays = [];

      const pauseWeekDays = [];
      const pauseWeekDayTypes = [];
      Gantt.calendar.weekDays.forEach((wd) => {
        if (!wd.dayWorking) {
          pauseWeekDayTypes.push(wd.dayType);
        }
      });
      Gantt.calendar.pauseWeekDayTypes = pauseWeekDayTypes;

      for (let i = 0; i < weekCount; i++) {
        const preWeekStartDay = minLineDay.clone().day(7 * i + weekStartDay);
        const preWeekStartDayFormat = preWeekStartDay.format('YYYY-MM-DD');
        weeksArry.push(preWeekStartDayFormat);

        for (let i = 0; i < 7; i++) {
          const day = moment.unix(preWeekStartDay.unix() + i * oneDayLong);
          // 当前日期是周几
          const dayWeekType = new Date(day.unix() * 1000).getDay();
          if (
            Gantt.calendar.pauseWeekDayTypes.indexOf(dayWeekType + 1) !== -1
          ) {
            pauseWeekDays.push(day.format('YYYY-MM-DD'));
          }
        }
      }
      Gantt.calendar.pauseWeekDays = pauseWeekDays;

      if (calenderArry.length === 0) {
        Gantt.calendar.calenderWidth = 0;
      } else {
        Gantt.calendar.weeksArry = weeksArry;
        Gantt.calendar.calenderWidth = weeksArry.length * 7 * baseWidth;
      }

      // 计算canvasInfo
      if (lastFinishTask) {
        // 清空关键线路
        GanttMethod.tasks.cleanKeyLoop(allTasks);
        // 重新计算关键线路
        GanttMethod.tasks.findKeyLoop(Gantt, allTasks, lastFinishTask);
      }
      GanttMethod.canvas.positionCalculate(Gantt, minLineDay);

      // 计算 actualCanvasInfo
      if (lastActualFinishTask) {
        GanttMethod.tasks.cleanActualKeyLoop(allTasks);
        GanttMethod.tasks.findActualKeyLoop(allTasks, lastActualFinishTask);
      }
      GanttMethod.canvas.positionCalculate(Gantt, minLineDay, 'actual');
    },

    positionCalculate(Gantt: IXmpp, minLineDay, dateType?: 'actual') {
      // 默认画轴起点坐标（0，0）
      // let tasks = this.showTask;
      const tasks = Gantt.allTasks;
      const canvasInfo = [];
      const baseWidth = Gantt.calendar.baseCellWidth;
      let posYadd = (Gantt.task.taskHeight - Gantt.draw.lineHeight) / 2;
      // 计算
      for (let i = 0; i < tasks.length; i++) {
        let isActive = false;
        const isKey = tasks[i].isKey;
        let startDate = moment(tasks[i].startDate);
        let endDate = moment(tasks[i].endDate);
        let duration = tasks[i].duration;
        let color = isKey
          ? Gantt.draw.color.planKeyColor
          : Gantt.draw.color.planColor;
        let lineHeight = Gantt.draw.lineHeight;
        // 计算实际时间的canvas信息
        if (dateType === 'actual') {
          startDate = moment(tasks[i].actualStartDate);
          endDate = moment(tasks[i].actualEndDate);
          duration = tasks[i].actualDuration;
          posYadd = (Gantt.task.taskHeight - Gantt.draw.actualLineHeight) / 2;
          color = isKey
            ? Gantt.draw.color.ActualkeyColor
            : Gantt.draw.color.Actualcolor;
          lineHeight = Gantt.draw.actualLineHeight;
        }
        if (
          Gantt.task.activeTaskId &&
          Gantt.task.activeTaskId === tasks[i].id
        ) {
          isActive = true;
        }
        const childTaskID = tasks[i].childTaskID;
        let taskHeight = Gantt.task.taskHeight;
        let isMilepost = tasks[i].isMilepost;
        // 找出task[i]之前fold的task个数
        const foldBefore = [];
        Gantt.task.hideTasksIds.forEach((element) => {
          if (element < i + 1) {
            foldBefore.push(element);
          }
        });
        // const pre = moment(startDate.format('YYYY-MM-DD'));
        // const minLineDayUnix = moment(minLineDay.format('YYYY-MM-DD'));
        const diff = startDate.clone().diff(minLineDay, 'days');
        // 里程碑width为0
        let positionX = diff * baseWidth;
        let width = (endDate.diff(startDate, 'days') + 1) * baseWidth;
        let type = 'normal';
        if (isMilepost && duration === 0) {
          width = 0;
          positionX = (diff + 1) * baseWidth;
          type = 'milepost';
        }
        if (childTaskID.length > 0) {
          type = 'parentType';
        }

        // 资源文本
        let resourceText = '';
        if (tasks[i].assignments.length > 0) {
          resourceText = tasks[i].assignmentsStr;
        }

        canvasInfo.push({
          // task起点x坐标
          positionX,
          // task起点y坐标
          positionY:
            taskHeight * i +
            posYadd -
            Gantt.task.startTaskIndex * taskHeight -
            foldBefore.length * taskHeight,
          // positionY: taskHeight * i + 10,
          // 根据工期算出来的task宽度
          width,
          // 横道图颜色
          color,
          arrowColor: isKey
            ? Gantt.draw.color.arrowKeyColor
            : Gantt.draw.color.arrowColor,
          // 是否是关键任务
          isKey,
          // 图形样式
          type,
          // 是否选中
          isActive,
          // 横道图高度
          lineHeight,
          resourceText,
        });
      }
      if (dateType === 'actual') {
        Gantt.draw.actualCanvasInfo = canvasInfo;
      } else {
        // 例外日期区域
        const exceptDate = Gantt.calendar.exceptDate;
        const exceptCanvasInfo = [];
        exceptDate.forEach((element) => {
          const positionX =
            moment(element.fromDate)
              .clone()
              .diff(moment(minLineDay).clone(), 'days') * baseWidth;
          const width =
            (moment(element.toDate).diff(moment(element.fromDate), 'days') +
              1) *
            baseWidth;
          exceptCanvasInfo.push({
            positionX,
            width,
          });
        });

        /** 工作周中休息的日期 */
        const pauseWeekDays = Gantt.calendar.pauseWeekDays;
        pauseWeekDays.forEach((pwd) => {
          let positionX =
            moment(pwd).diff(moment(minLineDay).clone(), 'days') * baseWidth;
          if (moment(minLineDay).format('YYYY-MM-DD') === pwd) {
            positionX = 0;
          }
          const width = baseWidth;
          exceptCanvasInfo.push({
            positionX,
            width,
          });
        });

        Gantt.draw.exceptCanvasInfo = exceptCanvasInfo;
        Gantt.draw.canvasInfo = canvasInfo;
      }
    },
    /**
     * 绘制额外日期区域
     * @param Gantt IXmpp
     * @param  ctx 画布
     */
    drawExceptArea(Gantt: IXmpp, ctx: CanvasRenderingContext2D) {
      const baseZoom = Gantt.draw.baseZoom ? Gantt.draw.baseZoom * 2 : 1 * 2;
      const exceptCanvasInfo = Gantt.draw.exceptCanvasInfo;
      if (Gantt.calendar.calenderWidth > 0) {
        for (const element of exceptCanvasInfo) {
          const fromX =
            element.positionX * baseZoom - Gantt.draw.canvasLeftHide;
          const width = element.width * baseZoom;
          ctx.beginPath();
          ctx.fillStyle = Gantt.draw.color.exceptDateColor;
          ctx.fillRect(fromX, 0, width, Gantt.draw.canvasHeight * 2);
          ctx.fill();
          ctx.closePath();
        }
      }
    },
    /**
     * 绘制任务举行
     * @param Gantt
     * @param ctx
     * @param isActual
     */
    drawTasks(Gantt: IXmpp, ctx: CanvasRenderingContext2D, isActual?: boolean) {
      const baseZoom = Gantt.draw.baseZoom ? Gantt.draw.baseZoom * 2 : 1 * 2;
      const hideTasksId = Gantt.task.hideTasksIds;
      let canvasInfo = Gantt.draw.canvasInfo;
      if (isActual) {
        canvasInfo = Gantt.draw.actualCanvasInfo;
      }
      for (let i = 0; i < Gantt.task.showTask.length; i++) {
        const currentTask = Gantt.task.showTask[i];
        const index = currentTask.id - 1;
        if (!canvasInfo[index]) {
          continue;
        }
        const mileStoneText = moment(currentTask.endDate).format('DD/MM');
        const fromX =
          canvasInfo[index].positionX * baseZoom - Gantt.draw.canvasLeftHide;
        const fromY = canvasInfo[index].positionY * 2;
        const width = canvasInfo[index].width * baseZoom;
        const isKey = canvasInfo[index].isKey;
        const type = canvasInfo[index].type;
        const color = canvasInfo[index].color;
        // const arrowColor = canvasInfo[index].arrowColor;
        const lineHeight = canvasInfo[index].lineHeight * 2;
        const isActive = canvasInfo[index].isActive;
        const resourceText = canvasInfo[index].resourceText;
        const fontSize = 12 * baseZoom;
        const arrowUnit = 5 * baseZoom;
        const lineWidth = 1.5 * baseZoom;
        const textFromX =
          baseZoom *
            (Gantt.draw.canvasInfo[index].positionX +
              Gantt.draw.canvasInfo[index].width +
              arrowUnit) -
          Gantt.draw.canvasLeftHide;

        if (isNaN(fromX)) {
          continue;
        }
        ctx.beginPath();
        if (currentTask.truePrevTaskID && !isActual) {
          const relations = currentTask.prevRelation;
          relations.forEach((relation) => {
            if (!relation.isDelete) {
              const number = relation.relation;
              const prevId = relation.prevId;
              const prevX =
                canvasInfo[prevId - 1].positionX * baseZoom -
                Gantt.draw.canvasLeftHide;
              const prevY = canvasInfo[prevId - 1].positionY * 2;
              const prevWith = canvasInfo[prevId - 1].width * baseZoom;

              const prevColor = canvasInfo[prevId - 1].color;
              const conectColor = prevColor;
              const helfLineHeight = lineHeight / 2;
              if (hideTasksId.indexOf(prevId) === -1) {
                if (number === PREVTYPE.FS) {
                  // 完成-开始(FS)
                  const points = [
                    { x: prevX + prevWith, y: prevY + helfLineHeight },
                    { x: fromX + arrowUnit, y: prevY + helfLineHeight },
                  ];
                  GanttMethod.canvas.drawBrokenLine(
                    ctx,
                    points,
                    lineWidth,
                    conectColor
                  );

                  GanttMethod.canvas.drawArrow(
                    ctx,
                    fromX + arrowUnit,
                    prevY + helfLineHeight,
                    fromX + arrowUnit,
                    fromY,
                    null,
                    null,
                    lineWidth,
                    conectColor
                  );
                  // GanttMethod.canvas.drawLineArrow(ctx, fromX + arrowUnit, prevY + helfLineHeight, fromX + arrowUnit, fromY - 4)
                }

                if (number === PREVTYPE.SS) {
                  // 开始-开始(SS)
                  const points = [
                    { x: prevX, y: prevY + helfLineHeight },
                    { x: prevX - arrowUnit - 10, y: prevY + helfLineHeight },
                    { x: prevX - arrowUnit - 10, y: fromY + helfLineHeight },
                  ];
                  GanttMethod.canvas.drawBrokenLine(
                    ctx,
                    points,
                    lineWidth,
                    conectColor
                  );
                  GanttMethod.canvas.drawArrow(
                    ctx,
                    prevX - arrowUnit - 10,
                    fromY + helfLineHeight,
                    fromX,
                    fromY + helfLineHeight,
                    null,
                    null,
                    lineWidth,
                    conectColor
                  );
                }

                if (number === PREVTYPE.FF) {
                  // 完成-完成(FF)
                  const points = [
                    { x: prevX + prevWith, y: prevY + helfLineHeight },
                    { x: fromX + width + arrowUnit, y: prevY + helfLineHeight },
                    { x: fromX + width + arrowUnit, y: fromY + helfLineHeight },
                  ];
                  GanttMethod.canvas.drawBrokenLine(
                    ctx,
                    points,
                    lineWidth,
                    conectColor
                  );
                  GanttMethod.canvas.drawArrow(
                    ctx,
                    fromX + width + arrowUnit,
                    fromY + helfLineHeight,
                    fromX + width,
                    fromY + helfLineHeight,
                    null,
                    null,
                    lineWidth,
                    conectColor
                  );
                }

                if (number === PREVTYPE.SF) {
                  // 开始-完成(SF)
                  const points = [
                    { x: prevX, y: prevY + helfLineHeight },
                    { x: prevX - arrowUnit, y: prevY + helfLineHeight },
                    { x: prevX - arrowUnit, y: prevY + lineHeight + arrowUnit },
                    {
                      x: fromX + width + arrowUnit,
                      y: prevY + lineHeight + arrowUnit,
                    },
                    { x: fromX + width + arrowUnit, y: fromY + helfLineHeight },
                  ];
                  GanttMethod.canvas.drawBrokenLine(
                    ctx,
                    points,
                    lineWidth,
                    conectColor
                  );
                  GanttMethod.canvas.drawArrow(
                    ctx,
                    fromX + width + arrowUnit,
                    fromY + helfLineHeight,
                    fromX + width,
                    fromY + helfLineHeight,
                    null,
                    null,
                    lineWidth,
                    conectColor
                  );
                }
              }
            }
          });
        }
        if (type === 'parentType') {
          if (!isActual) {
            // 父任务
            const toX = fromX + width;
            const toY = fromY;
            GanttMethod.canvas.drawSenior(ctx, fromX, fromY, toX, toY);
          }
        } else if (type === 'milepost') {
          // 里程碑
          GanttMethod.canvas.drawPolygon(ctx, {
            x: fromX,
            y: fromY + 13,
            num: 4,
            r: 6 * 2,
            width: 1,
            fillStyle: color,
          });
          GanttMethod.canvas.drawText(
            ctx,
            mileStoneText,
            fromX + arrowUnit,
            fromY + arrowUnit,
            fontSize,
            '#000000',
            'bold'
          );
        } else {
          // 普通任务
          ctx.fillStyle = color;
          ctx.fillRect(fromX, fromY, width, lineHeight);
          ctx.fill();
          if (!isActual) {
            // tslint:disable-next-line:max-line-length
            // const planfromX = 2*(Gantt.draw.canvasInfo[index].positionX - Gantt.draw.canvasLeftHide + Gantt.draw.canvasInfo[index].width + arrowUnit);
            // // tslint:disable-next-line:max-line-length
            // const actualfromX = Gantt.draw.actualCanvasInfo[index].positionX - Gantt.draw.canvasLeftHide + Gantt.draw.actualCanvasInfo[index].width + arrowUnit;
            // let textFromX = planfromX;
            // if (actualfromX > planfromX) {
            //     textFromX = actualfromX;
            // }

            GanttMethod.canvas.drawText(
              ctx,
              resourceText,
              textFromX,
              fromY + arrowUnit,
              fontSize,
              '#000000',
              'bold'
            );
          }
        }
        ctx.closePath();

        // 选中任务
        // if (isActive) {
        //     const startY = fromY - (Gantt.task.taskHeight - lineHeight) / 2;
        //     const endY = fromY + Gantt.task.taskHeight - (Gantt.task.taskHeight - lineHeight) / 2;
        //     GanttMethod.canvas.drawLine(ctxMask, 0, startY + 1, canvasWidth, startY, 1, '#ccc');
        //     GanttMethod.canvas.drawLine(ctxMask, 0, endY - 1, canvasWidth, endY, 1, '#ccc');
        // }

        // 前置任务到自己的箭头
      }
    },
    /**
     * 在mask canvas上绘制选中任务的线
     * @param Gantt
     * @param index 选中任务在canvasInfo中的index
     */
    drawSelectTask(Gantt: IXmpp, taskId: number) {
      Gantt.draw.selectedTaskId = taskId;
      // this.Xmpp.draw.ctxMask
      // this.Xmpp.draw.ctxMask.scale(2, 2);
      // ctxMask.clearRect(0, 0, Gantt.draw.canvasWidth, Gantt.draw.canvasHeight);
      const index = taskId - 1;
      const canvasWidth = Gantt.draw.canvasWidth;
      const canvasInfo = Gantt.draw.canvasInfo;
      if (index !== -1 && canvasInfo[index]) {
        const fromY = canvasInfo[index].positionY;
        const lineHeight = canvasInfo[index].lineHeight;
        const startY = (fromY - (Gantt.task.taskHeight - lineHeight) / 2) * 2;
        const endY =
          (fromY +
            Gantt.task.taskHeight -
            (Gantt.task.taskHeight - lineHeight) / 2) *
          2;
        GanttMethod.canvas.drawLine(
          Gantt.draw.ctxMask,
          0,
          startY + 1,
          canvasWidth * 2,
          startY,
          2,
          '#ccc'
        );
        GanttMethod.canvas.drawLine(
          Gantt.draw.ctxMask,
          0,
          endY - 1,
          canvasWidth * 2,
          endY,
          2,
          '#ccc'
        );
      }
    },
    /**
     * 绘制圆角矩形
     * @param cxt 画布
     * @param x 位置x
     * @param y 位置y
     * @param width 宽度
     * @param height 高度
     * @param radius 圆角
     */
    drawRoundRect(cxt, x, y, width, height, radius) {
      cxt.beginPath();
      cxt.arc(x + radius, y + radius, radius, Math.PI, (Math.PI * 3) / 2);
      cxt.lineTo(width - radius + x, y);
      cxt.arc(
        width - radius + x,
        radius + y,
        radius,
        (Math.PI * 3) / 2,
        Math.PI * 2
      );
      cxt.lineTo(width + x, height + y - radius);
      cxt.arc(
        width - radius + x,
        height - radius + y,
        radius,
        0,
        (Math.PI * 1) / 2
      );
      cxt.lineTo(radius + x, height + y);
      cxt.arc(
        radius + x,
        height - radius + y,
        radius,
        (Math.PI * 1) / 2,
        Math.PI
      );
      cxt.closePath();
    },
    /**
     * 绘制文本
     * @param ctx 画布
     * @param text 文本内容
     * @param x 位置x
     * @param y 位置y
     * @param fontSize 字体大小
     */
    drawText(
      ctx,
      text: string,
      x: number,
      y: number,
      fontSize,
      fillStyle = '#000000',
      fontWeight = 'normal'
    ) {
      ctx.beginPath();
      ctx.font = `normal normal ${fontWeight} ${fontSize}px arial`;
      ctx.textBaseline = 'middle';
      if (fillStyle) {
        ctx.fillStyle = fillStyle;
        ctx.fill();
      }
      ctx.fillText(text, x, y);
      ctx.closePath();
    },
    /**
     * 绘制里程碑
     * @param ctx 画布
     * @param conf 配置
     */
    drawPolygon(ctx, conf) {
      const x = (conf && conf.x) || 0; // 中心点x坐标
      const y = (conf && conf.y) || 0; // 中心点y坐标
      const num = (conf && conf.num) || 3; // 图形边的个数
      const r = (conf && conf.r) || 6; // 图形的半径
      const width = (conf && conf.width) || 1;
      const strokeStyle = conf && conf.strokeStyle;
      const fillStyle = conf && conf.fillStyle;
      // 开始路径
      ctx.beginPath();
      const startX = x + r * Math.cos((2 * Math.PI * 0) / num);
      const startY = y + r * Math.sin((2 * Math.PI * 0) / num);
      ctx.moveTo(startX, startY);
      for (let i = 1; i <= num; i++) {
        const newX = x + r * Math.cos((2 * Math.PI * i) / num);
        const newY = y + r * Math.sin((2 * Math.PI * i) / num);
        ctx.lineTo(newX, newY);
      }
      ctx.closePath();
      // 路径闭合
      if (strokeStyle) {
        ctx.strokeStyle = strokeStyle;
        ctx.lineWidth = width;
        ctx.lineJoin = 'round';
        ctx.stroke();
      }
      if (fillStyle) {
        ctx.fillStyle = fillStyle;
        ctx.fill();
      }
    },
    /**
     * 绘制父任务的括号
     * @param ctx 画布
     * @param fromX 开始x
     * @param fromY 开始y
     * @param toX 结束x
     * @param toY 结束y
     */
    drawSenior(ctx, fromX, fromY, toX, toY) {
      const width = 4;
      const color = '#707070';
      GanttMethod.canvas.drawLine(
        ctx,
        fromX,
        fromY + 10,
        fromX,
        toY,
        width,
        color
      );
      GanttMethod.canvas.drawLine(ctx, fromX, fromY, toX, toY, width, color);
      GanttMethod.canvas.drawLine(ctx, toX, toY, toX, toY + 10, width, color);
    },
    /**
     * @param ctx Canvas绘图环境
     * @param fromX fromY：起点坐标（也可以换成p1，只不过它是一个数组）
     * @param toX toY：终点坐标 (也可以换成p2，只不过它是一个数组)
     * @param theta 三角斜边一直线夹角
     * @param headlen：三角斜边长度
     * @param width 箭头线宽度
     */
    drawArrow(
      ctx,
      fromX,
      fromY,
      toX,
      toY,
      theta?: number,
      headlen?: number,
      width?: number,
      color?: string
    ) {
      theta = theta ? theta : 30;
      headlen = headlen ? headlen : 15;
      width = width ? width : 2;

      // 计算各角度和对应的p2,p3坐标
      const angle = (Math.atan2(fromY - toY, fromX - toX) * 180) / Math.PI;
      const angle1 = ((angle + theta) * Math.PI) / 180;
      const angle2 = ((angle - theta) * Math.PI) / 180;
      const topX = headlen * Math.cos(angle1);
      const topY = headlen * Math.sin(angle1);
      const botX = headlen * Math.cos(angle2);
      const botY = headlen * Math.sin(angle2);

      ctx.save();
      ctx.beginPath();

      let arrowX = fromX - topX;
      let arrowY = fromY - topY;

      ctx.moveTo(arrowX, arrowY);
      ctx.moveTo(fromX, fromY);
      ctx.lineTo(toX, toY);

      arrowX = toX + topX;
      arrowY = toY + topY;
      ctx.moveTo(arrowX, arrowY);
      ctx.lineTo(toX, toY);
      arrowX = toX + botX;
      arrowY = toY + botY;
      ctx.lineTo(toX, toY);
      arrowX = toX + botX;
      arrowY = toY + botY;
      ctx.lineTo(arrowX, arrowY);
      ctx.lineWidth = width;
      ctx.stroke();
      ctx.fillStyle = color;
      ctx.fill();
      ctx.closePath();
    },
    /**
     * 绘制折线
     * @param ctx 画布
     * @param points 折线上的点数组
     * @param width 宽度
     * @param color 颜色
     */
    drawBrokenLine(ctx, points: any[], width?: number, color?: string) {
      // 绘制直线
      ctx.beginPath();
      ctx.strokeStyle = color;
      // 设置线条的宽度
      ctx.lineWidth = width ? width : 1;

      // 起点
      ctx.moveTo(points[0].x, points[0].y);
      // 路径
      for (const point of points) {
        ctx.lineTo(point.x, point.y);
      }
      ctx.stroke();
    },
    /**
     * 绘制直线
     * @param ctx 画布
     * @param fromX 开始x
     * @param fromY 开始y
     * @param toX 结束x
     * @param toY 结束y
     * @param width 宽度
     * @param color 颜色
     */
    drawLine(
      ctx: CanvasRenderingContext2D,
      fromX,
      fromY,
      toX,
      toY,
      width,
      color
    ) {
      color = typeof color !== 'undefined' ? color : '#000';
      ctx.save();
      ctx.beginPath();
      ctx.strokeStyle = color;
      ctx.lineWidth = width;
      ctx.moveTo(fromX - 0.5, fromY - 0.5);
      ctx.lineTo(toX - 0.5, toY - 0.5);
      ctx.stroke();
      ctx.restore();
    },
  };

  static tasks = {
    // 初始化关键线路isKey = false
    cleanKeyLoop(tasks: Array<XmppTask>) {
      for (const element of tasks) {
        element.isKey = false;
      }
    },

    cleanActualKeyLoop(tasks: Array<XmppTask>) {
      for (const element of tasks) {
        element.isActualKey = false;
      }
    },

    findActualKeyLoop(tasks: Array<XmppTask>, key: number) {
      tasks[key - 1].isActualKey = true;
      const keyWork = tasks[key - 1].actulaTruePrevTaskID;
      if (keyWork) {
        GanttMethod.tasks.findActualKeyLoop(tasks, keyWork);
      } else {
        return;
      }
    },
    // 循环查找关键线路
    findKeyLoop(Gantt: IXmpp, tasks: Array<XmppTask>, key: number) {
      tasks[key - 1].isKey = true;
      if (tasks[key - 1].childTaskID.length > 0) {
        const childTaskID = tasks[key - 1].childTaskID;
        const allTasks = Gantt.allTasks;
        childTaskID.forEach((id) => {
          if (allTasks[id - 1].endDate === tasks[key - 1].endDate) {
            allTasks[id - 1].isKey = true;
          }
        });
      }
      const keyWork = tasks[key - 1].truePrevTaskID;
      if (keyWork) {
        GanttMethod.tasks.findKeyLoop(Gantt, tasks, keyWork);
      } else {
        return;
      }
    },

    updateTaskHandle(Gantt: IXmpp) {
      const startIndex = Gantt.task.startTaskIndex;
      const endIndex = Gantt.task.startTaskIndex + Gantt.task.showTaskLength;

      if (Gantt.allTasks && Gantt.allTasks.length > 0) {
        const allTasks = GanttMethod.tasks.getAllTaskAfterFold(Gantt);
        Gantt.task.showTask = allTasks.slice(startIndex, endIndex);
      } else {
        Gantt.task.showTask = [];
      }
    },

    getAllTaskAfterFold(Gantt: IXmpp) {
      const allTasks = Gantt.allTasks;
      const newAllTasks = [];
      const hideTasksId = [];
      allTasks.forEach((task, index) => {
        if (task.isFold) {
          for (let m = index + 1; m < allTasks.length; m++) {
            const element = allTasks[m];
            if (element.level > allTasks[index].level) {
              if (hideTasksId.indexOf(m + 1) === -1) {
                hideTasksId.push(element.id);
              } else {
                return;
              }
            } else {
              break;
            }
          }
        }
      });

      Gantt.task.hideTasksIds = hideTasksId;
      for (const element of allTasks) {
        if (hideTasksId.indexOf(element.id) === -1) {
          newAllTasks.push(element);
        }
      }
      return newAllTasks;
    },
    /*
     * 更新levelInfo
     * 根据level计算childTaskID和parentTaskID
     * major fuction
     */
    updateLeveInfo(allTasks) {
      allTasks.forEach((element) => {
        element.childTaskID = [];
        element.parentTaskID = null;
      });
      // 计算所有任务的childTaskID和parentTaskID
      allTasks.forEach((task, index) => {
        const nextIndex = index + 1;
        GanttMethod.tasks.levelLoop(allTasks, task, nextIndex);
      });

      // 降级后，子任务的前置任务变成了它的父任务，则移除该前置任务
      for (const element of allTasks) {
        const childrenId = element.childTaskID;
        childrenId.forEach((id) => {
          if (!allTasks[id - 1]) {
            console.log(id);
          }
          GanttMethod.tasks.removeParentID(
            allTasks,
            allTasks[id - 1],
            element.id
          );
        });
      }
    },

    levelLoop(allTasks, task, nextIndex) {
      const level = task.level;
      if (allTasks[nextIndex] && allTasks[nextIndex].level > level) {
        if (allTasks[nextIndex].level === level + 1) {
          task.childTaskID.push(allTasks[nextIndex].id);
          allTasks[nextIndex].parentTaskID = task.id;
        }
        nextIndex++;
        GanttMethod.tasks.levelLoop(allTasks, task, nextIndex);
      } else {
        return;
      }
    },

    removeParentID(allTasks, children: XmppTask, parentId: number) {
      const childArray = children.prevRelation;
      if (childArray && childArray.length > 0) {
        const check = childArray.findIndex((relation) => {
          return relation.prevId === parentId;
        });
        if (check !== -1) {
          childArray.splice(check, 1);
        }
        if (children.childTaskID.length !== 0) {
          for (const child of children.childTaskID) {
            const element = allTasks[child - 1];
            GanttMethod.tasks.removeParentID(allTasks, element, parentId);
          }
        }
      } else {
        return;
      }
    },

    addTaskHandle(Gantt: IXmpp, taskParam: XmppTask) {
      const allTasks = Gantt.allTasks;
      // const firstSelectTask = allTasks.find((task) => {
      //     return task.isSelected === true;
      // });
      if (taskParam && taskParam.isMilepost) {
        taskParam.taskName = '里程碑';
      }
      const activeTask = Gantt.task.activeTaskId
        ? allTasks[Gantt.task.activeTaskId - 1]
        : null;
      if (!activeTask) {
        const newTask = new XmppTask({
          Xmpp: Gantt,
          id: allTasks.length + 1,
          symbol: allTasks.length + 1000,
          ganttChartId: Gantt.mpp.mppInfo.id,
          taskName:
            taskParam && taskParam.taskName ? taskParam.taskName : '新建任务',
          isSelected: false,
          startDate:
            taskParam && taskParam.startDate ? taskParam.startDate : null,
          duration: 1,
          endDate: taskParam && taskParam.endDate ? taskParam.endDate : null,
          actualStartDate: null,
          actualDuration: 0,
          actualEndDate: null,
          childTaskID: [],
          parentTaskID: null,
          isMilepost:
            (taskParam && taskParam.isMilepost) ? taskParam.isMilepost : false,
          level: 1,
          bindings: [],
          prevRelation: [],
          finishRate: 0,
          exceptDate: Gantt.calendar.exceptDate
            ? Gantt.calendar.exceptDate
            : [],
        });
        allTasks.push(newTask);
      } else {
        const firstSelectIndex = activeTask.id - 1;
        const newTask = new XmppTask({
          Xmpp: Gantt,
          id: firstSelectIndex + 1,
          symbol: allTasks.length + 1000,
          ganttChartId: Gantt.mpp.mppInfo.id,
          taskName:
            taskParam && taskParam.taskName ? taskParam.taskName : '新建任务',
          isSelected: false,
          startDate:
            taskParam && taskParam.startDate ? taskParam.startDate : null,
          duration: 1,
          endDate: taskParam && taskParam.endDate ? taskParam.endDate : null,
          actualStartDate: null,
          actualDuration: 0,
          actualEndDate: null,
          childTaskID: [],
          parentTaskID: null,
          isMilepost:
           (taskParam && taskParam.isMilepost ) ? taskParam.isMilepost : false,
          level: activeTask.level,
          bindings: [],
          prevRelation: [],
          finishRate: 0,
          exceptDate: Gantt.calendar.exceptDate
            ? Gantt.calendar.exceptDate
            : [],
        });
        allTasks.splice(firstSelectIndex, 0, newTask);
        for (const task of allTasks) {
          const relation = task.prevRelation;
          relation.forEach((element) => {
            if (element.prevId > firstSelectIndex) {
              element.prevId = element.prevId + 1;
            }
          });
        }
      }
      GanttMethod.tasks.loopAllTasksId(Gantt);
      // 涉及到两个数组
    },

    addTask(currentMpp: IXmpp) {
      const selectedTask = currentMpp.task.selectedTasks[0];
      let taskId = currentMpp.allTasks.length + 1;
      let taskLevel = 1;
      if (currentMpp.allTasks.length > 0) {
        taskLevel = currentMpp.allTasks[currentMpp.allTasks.length - 1].level;
      }
      if (selectedTask) {
        taskId = selectedTask.id - 1;
        taskLevel = selectedTask.level;
      }
      const date = moment(currentMpp.mpp.mppInfo.startDate).format(
        'YYYY-MM-DD'
      );
      // if (currentMpp.allTasks.length > 0) {
      //     date = moment(currentMpp.calendar.minLineDay).format('YYYY-MM-DD')
      // }
      const newTask = new XmppTask({
        uid: GanttMethod.mpp.findMaxUid(currentMpp.allTasks) + 1,
        Xmpp: currentMpp,
        id: taskId,
        symbol: currentMpp.allTasks.length + 1000,
        taskName: '新建任务',
        isSelected: false,
        startDate: date,
        duration: 1,
        endDate: date,
        actualStartDate: null,
        actualDuration: 0,
        actualEndDate: null,
        childTaskID: [],
        parentTaskID: null,
        isMilepost: false,
        level: taskLevel,
        prevRelation: [],
        assignments: [],
      });

      if (selectedTask) {
        currentMpp.allTasks.splice(taskId, 0, newTask);
        for (const task of currentMpp.allTasks) {
          const relation = task.prevRelation;
          relation.forEach((element) => {
            if (element.prevId > taskId) {
              element.prevId = element.prevId + 1;
            }
          });
        }
      } else {
        currentMpp.allTasks.push(newTask);
      }
    },

    deleteTaskHandle(Gantt: IXmpp, deleteTaskIds: number[]) {
      const allTasks = Gantt.allTasks;
      // 将选中的tasks从allTasks移除
      for (let i = allTasks.length - 1; i >= 0; i--) {
        const id = allTasks[i].id;
        const findId = deleteTaskIds.find((task, index) => {
          return task === id;
        });
        if (findId) {
          const task = allTasks[findId - 1];
          if (task.sqId) {
            Gantt.task.deleteTasksSqlIdStore.push(task.sqId);
          }
          allTasks.splice(findId - 1, 1);
        }
      }

      // 循环删除任务的id数组
      for (const task of allTasks) {
        // 第一次循环，删除relation中已删除的任务对应的relation
        const prevRelation = task.prevRelation;
        prevRelation.forEach((relation, index) => {
          if (deleteTaskIds.indexOf(relation.prevId) !== -1) {
            prevRelation.splice(index, 1);
          }
        });

        // 第二次循环,比删除任务id大的prevId-1
        for (const relation of prevRelation) {
          let prevId = relation.prevId;
          const smallprev = [];
          deleteTaskIds.forEach((id) => {
            if (id < prevId) {
              smallprev.push(id);
            }
          });
          if (smallprev.length > 0) {
            prevId = prevId - smallprev.length;
            relation.prevId = prevId;
          }
        }
      }
      GanttMethod.tasks.loopAllTasksId(Gantt);
    },

    depressTaskLevel(Gantt: IXmpp, selectTasks: any[]) {
      // const selectTasks = GanttMethod.tasks.checkSelectNumber(Gantt.allTasks);
      if (selectTasks && selectTasks.length > 0) {
        // 拼装depressTasksId
        const search = (element, depressTasksId) => {
          if (element.childTaskID && element.childTaskID.length > 0) {
            element.childTaskID.forEach((childID) => {
              if (depressTasksId.indexOf(childID) === -1) {
                depressTasksId.push(childID);
              }
              search(Gantt.allTasks[childID - 1], depressTasksId);
            });
          } else {
            return;
          }
        };
        selectTasks.forEach((task) => {
          // 任务leve + 1
          const depressTasksId = [];
          depressTasksId.push(task.id);
          search(task, depressTasksId);
          depressTasksId.forEach((id) => {
            const upperTask = Gantt.allTasks[id - 2];
            if (Gantt.allTasks[id - 1].level > upperTask.level) {
              return;
            } else {
              Gantt.allTasks[id - 1].level = Gantt.allTasks[id - 1].level + 1;
            }
          });
        });
        Gantt.render();
      }
    },

    promoteTaskLevel(Gantt: IXmpp, selectTasks: any) {
      // const selectTasks = GanttMethod.tasks.checkSelectNumber(Gantt.allTasks);

      if (selectTasks) {
        // 拼装promoteTasksId
        const search = (element, promoteTasksId) => {
          if (element.childTaskID && element.childTaskID.length > 0) {
            element.childTaskID.forEach((childID) => {
              if (promoteTasksId.indexOf(childID) === -1) {
                promoteTasksId.push(childID);
              }
              search(Gantt.allTasks[childID - 1], promoteTasksId);
            });
          } else {
            return;
          }
        };

        selectTasks.forEach((task) => {
          const promoteTasksId = [];
          // 任务leve + 1
          promoteTasksId.push(task.id);
          search(task, promoteTasksId);
          promoteTasksId.forEach((id) => {
            if (Gantt.allTasks[id - 1].level > 1) {
              Gantt.allTasks[id - 1].level = Gantt.allTasks[id - 1].level - 1;
            }
          });
        });
        Gantt.render();
      }
    },

    checkSelectNumber(allTasks) {
      const selectNumber = [];
      allTasks.forEach((element) => {
        if (element.isSelected) {
          selectNumber.push(element);
        }
      });
      return selectNumber;
    },

    loopAllTasksId(Gantt: IXmpp) {
      for (let i = 0; i < Gantt.allTasks.length; i++) {
        const element = Gantt.allTasks[i];
        element.id = i + 1;
      }
      Gantt.render();
    },
  };

  static predecessorLink = {
    parentLoop(
      allTasks,
      ownPrevRelation: XmppPredecessorLink[],
      currentTask: XmppTask
    ) {
      const parentIndex = currentTask.parentTaskID - 1;
      const parentPrev = allTasks[parentIndex].prevRelation;
      for (const prev of parentPrev) {
        if (ownPrevRelation.indexOf(prev) === -1) {
          ownPrevRelation.push(prev);
        }
      }
      if (allTasks[parentIndex].parentTaskID) {
        GanttMethod.predecessorLink.parentLoop(
          allTasks,
          ownPrevRelation,
          allTasks[parentIndex]
        );
      }
    },

    filterDeleteRelation(relation: XmppPredecessorLink[]) {
      const newArray: XmppPredecessorLink[] = [];
      relation.forEach((element) => {
        if (!element.isDelete) {
          newArray.push(element);
        }
      });
      return newArray;
    },

    loopParentMaxMin(
      allTasks,
      checkTask,
      task,
      laterChildId?,
      earlierChildId?
    ) {
      if (task.childTaskID.length > 0) {
        if (!laterChildId) {
          laterChildId = task.childTaskID[0];
        }
        if (!earlierChildId) {
          earlierChildId = task.childTaskID[0];
        }
        task.childTaskID.forEach((id) => {
          const child = allTasks[id - 1];
          const maxEndDate = allTasks[laterChildId - 1].endDate;
          const minStartDate = allTasks[earlierChildId - 1].startDate;
          if (child.childTaskID.length === 0) {
            if (moment(child.endDate).isAfter(maxEndDate)) {
              laterChildId = child.id;
            }
            if (moment(child.startDate).isBefore(minStartDate)) {
              earlierChildId = child.id;
            }
            checkTask.laterChildId = laterChildId;
            checkTask.earlierChildId = earlierChildId;
          } else {
            GanttMethod.predecessorLink.loopParentMaxMin(
              allTasks,
              checkTask,
              child,
              laterChildId,
              earlierChildId
            );
          }
        });
      }
    },

    dealWithPrev(task: IMppTask, allTasks: IMppTask[]) {
      const PredecessorLink = task.predecessorLink;
      const preTask = [];
      const finderId = (PredecessorUID) => {
        const finder = allTasks.find((ele) => {
          return ele.uid === PredecessorUID;
        });
        if (finder) {
          return finder._ID;
        }
      };
      if (PredecessorLink && PredecessorLink.length > 0) {
        PredecessorLink.forEach((element) => {
          const prevTask = new XmppPredecessorLink({
            prevId: finderId(element.predecessorUID),
            relation: element.type,
            delay: element.linkLag ? element.linkLag / 600 / 8 : 0,
            id: element.id,
            defaultPrev: element,
          });
          preTask.push(prevTask);
        });
      }
      return preTask;
    },
  };

  static date = {
    dateDeepClone(date) {
      return moment(date).format('YYYY-MM-DD');
    },
    /*
     *  根据前置任务计算allTasks的startDate
     */
    updateStartDate(Gantt: IXmpp) {
      const allTasks = Gantt.allTasks;
      for (let i = 0; i < Gantt.allTasks.length; i++) {
        const relation = Gantt.allTasks[i].prevRelation;
        const task = allTasks[i];

        let ownPrevRelation = [];
        // 自己的前置任务relation
        relation.forEach((element) => {
          ownPrevRelation.push(element);
        });
        // 如果有父任务，加入父任务的relation
        if (task.parentTaskID) {
          GanttMethod.predecessorLink.parentLoop(
            Gantt.allTasks,
            ownPrevRelation,
            task
          );
        }
        // 计算最大的startDate
        let maxStartDate: any;
        // 筛选掉isDelete的relation
        ownPrevRelation =
          GanttMethod.predecessorLink.filterDeleteRelation(ownPrevRelation);
        if (ownPrevRelation.length > 0) {
          let truePrevTaskID;

          for (const relationInfo of ownPrevRelation) {
            const preTaskIndex = relationInfo.prevId - 1;
            const preTask = allTasks[preTaskIndex];
            const delay = relationInfo.delay;
            const duration = task.duration;
            let startDate: any;
            let prevType = 1; // 1为普通任务，2为摘要任务
            if (!preTask) {
              continue;
            }
            try {
              if (preTask && preTask.childTaskID.length > 0) {
                // 前置任务是摘要任务
                GanttMethod.predecessorLink.loopParentMaxMin(
                  Gantt.allTasks,
                  preTask,
                  preTask
                );
                prevType = 2;
                const prevEndDate = allTasks[preTask.laterChildId - 1].endDate;
                const prevStartDate =
                  allTasks[preTask.earlierChildId - 1].startDate;
                switch (relationInfo.relation) {
                  // 完成-开始(FS)
                  case PREVTYPE.FS:
                    startDate = prevEndDate
                      ? moment(prevEndDate).clone().add(1, 'days').toDate()
                      : null;
                    break;
                  // 开始-开始(SS)
                  case PREVTYPE.SS:
                    startDate = prevStartDate
                      ? moment(prevStartDate).clone().add(1, 'days').toDate()
                      : null;
                    break;
                  // 完成-完成(FF)
                  case PREVTYPE.FF:
                    startDate = prevEndDate
                      ? moment(prevEndDate)
                          .clone()
                          .subtract(duration - 1, 'days')
                          .toDate()
                      : null;
                    break;
                  // 开始-完成(SF)
                  case PREVTYPE.SF:
                    startDate = prevStartDate
                      ? moment(prevStartDate)
                          .clone()
                          .subtract(duration - 1, 'days')
                          .toDate()
                      : null;
                    break;
                  default:
                    return;
                }
              } else {
                prevType = 1;
                switch (relationInfo.relation) {
                  // 完成-开始(FS)
                  case PREVTYPE.FS:
                    if (task.isMilepost && task.duration === 0) {
                      // 前置任务是里程碑,则startDate不加1
                      startDate = preTask.endDate
                        ? moment(preTask.endDate)
                            .clone()
                            .add(delay, 'days')
                            .toDate()
                        : null;
                    } else {
                      // 前置任务是普通任务,则startDate加1
                      startDate = preTask.endDate
                        ? moment(preTask.endDate)
                            .clone()
                            .add(delay + 1, 'days')
                            .toDate()
                        : null;
                    }
                    break;
                  // 开始-开始(SS)
                  case PREVTYPE.SS:
                    startDate = preTask.startDate
                      ? moment(preTask.startDate)
                          .clone()
                          .add(delay, 'days')
                          .toDate()
                      : null;
                    break;
                  // 完成-完成(FF)
                  case PREVTYPE.FF:
                    startDate = preTask.endDate
                      ? moment(preTask.endDate)
                          .clone()
                          .add(delay, 'days')
                          .subtract(duration - 1, 'days')
                          .toDate()
                      : null;
                    break;
                  // 开始-完成(SF)
                  case PREVTYPE.SF:
                    startDate = preTask.startDate
                      ? moment(preTask.startDate)
                          .clone()
                          .add(delay, 'days')
                          .subtract(duration - 1, 'days')
                          .toDate()
                      : null;
                    break;
                  default:
                    return;
                }
              }
              // 第一次maxStartDate为null, 默认为第一个计算的startDate
              if (!maxStartDate) {
                maxStartDate = startDate;
                truePrevTaskID =
                  prevType === 2 ? preTask.laterChildId : relationInfo.prevId;
              } else {
                if (moment(startDate).isAfter(maxStartDate)) {
                  maxStartDate = startDate;
                  truePrevTaskID =
                    prevType === 2 ? preTask.laterChildId : relationInfo.prevId;
                }
              }
            } catch (error) {}
          }
          task.truePrevTaskID = truePrevTaskID;
        } else {
          task.truePrevTaskID = null;
        }

        if (task.parentTaskStore && task.childTaskID.length === 0) {
          // 还原摘要成为普通任务时，还原任务时间
          const store = JSON.parse(task.parentTaskStore);
          task.startDate = store.startDate;
          task.duration = store.duration;
          task.endDate = store.endDate;
          task.parentTaskStore = null;
        } else if (
          !Gantt.task.isFirstInit &&
          maxStartDate &&
          !moment(task.startDate).isAfter(maxStartDate)
        ) {
          GanttMethod.date.setTaskStartDate(Gantt, task, maxStartDate);
        }

        // 判断有没有受例外日期的影响
        // const correctStartDate = GanttMethod.date.datePipe(Gantt, maxStartDate);
        // task.startDate = correctStartDate;
        // task.startDate = maxStartDate;

        // task.endDate = moment(task.startDate).add(task.duration - 1, 'days').toDate();

        // const startUnix = task.startDate ? moment(task.startDate).unix() : null;
        // const endUnix = task.endDate ? moment(task.endDate).unix() : null;
        // // 先根据例外日期，改变任务的开始时间和结束时间
        // console.log(Gantt.calendar.exceptDate)
        // Gantt.calendar.exceptDate.forEach((exceptDate) => {
        //     // 任务开始时间在例外日期之间
        //     let exceptDateStart = moment(exceptDate.startDate).unix();
        //     let exceptDateEnd = moment(exceptDate.endDate).unix()
        //     if (startUnix <= exceptDateEnd && startUnix >= exceptDateStart) {
        //         const correctDate = moment.unix(exceptDateEnd).clone().add(1, 'days').toDate();
        //         if (moment(correctDate).clone().isAfter(moment(maxStartDate))) {
        //             task.startDate = correctDate;
        //             // maxStartDate = correctDate;
        //         }
        //     }
        //     // 任务结束时间在例外日期之间
        //     if (endUnix <= exceptDateEnd && endUnix >= exceptDateStart) {
        //         const correctDate = moment.unix(exceptDateEnd).clone().add(1, 'days').toDate();
        //         task.endDate = correctDate;
        //     }
        // });
      }
    },
    updateParentStartDate(allTasks: XmppTask[]) {
      for (let i = allTasks.length - 1; i >= 0; i--) {
        if (allTasks[i].childTaskID.length > 0) {
          const parentTask = allTasks[i];
          GanttMethod.predecessorLink.loopParentMaxMin(
            allTasks,
            parentTask,
            parentTask
          );
          const minStartDate =
            allTasks[parentTask.earlierChildId - 1].startDate;
          const maxEndDate = allTasks[parentTask.laterChildId - 1].endDate;

          const store = {
            startDate: allTasks[i].startDate,
            duration: allTasks[i].duration,
            endDate: allTasks[i].endDate,
          };

          if (!allTasks[i].parentTaskStore) {
            allTasks[i].parentTaskStore = JSON.stringify(store);
          }

          allTasks[i].startDate = minStartDate;
          allTasks[i].endDate = maxEndDate;

          allTasks[i].duration =
            moment(maxEndDate).clone().diff(moment(minStartDate), 'days') + 1;
        }
      }
    },
    // dealWithExceptDate(Gantt: IXmpp) {
    //     const allTasks = Gantt.allTasks;
    //     for (const task of allTasks) {
    //         const startUnix2 = task.startDate ? moment(task.startDate).unix() : null;
    //         const endUnix2 = task.endDate ? moment(task.endDate).unix() : null;
    //         let allExcept = 0;
    //         // 再统计任务中所有的例外日期工期
    //         Gantt.calendar.exceptDate.forEach((item) => {
    //             let fromDateUnix = moment(item.fromDate).unix();
    //             let toDateUnix = moment(item.toDate).unix();
    //             // 例外日期在任务的开始时间和结束时间之间
    //             if (startUnix2 <= fromDateUnix && endUnix2 >= toDateUnix) {
    //                 const exceptDuration = moment(item.toDate).clone().diff(moment(item.fromDate), 'days') + 1;
    //                 allExcept = allExcept + exceptDuration;
    //             }
    //         });
    //         task.exceptDuration = allExcept;
    //         // 计算showDuration
    //         if (task.duration) {
    //             task.showDuration = task.duration - allExcept;
    //         } else {
    //             if (task.startDate && task.endDate) {
    //                 const num = moment(task.endDate).clone().diff(moment(task.startDate), 'days');
    //                 task.showDuration = num + 1;
    //             }
    //         }
    //     }
    // },
    // 设置开始时间
    setTaskStartDate(Gantt: IXmpp, task: XmppTask, startDate: string) {
      const dayUnix = moment(startDate).unix();
      const isPausedDay = GanttMethod.date.isPausedDay(Gantt, dayUnix);
      if (isPausedDay) {
        startDate = GanttMethod.date.datePipe(Gantt, startDate, 1);
      }
      task.endDate = moment(startDate)
        .add(task.duration - 1, 'days')
        .format('YYYY-MM-DD');
      const endDate = GanttMethod.date.handleEndDate(
        Gantt,
        startDate,
        task.duration,
        task.endDate
      );
      task.endDate = endDate;
      task.startDate = startDate;
    },
    // 设置结束时间
    setTaskEndDate(Gantt: IXmpp, task: XmppTask, endDate: string) {
      const dayUnix = moment(endDate).unix();
      const isPausedDay = GanttMethod.date.isPausedDay(Gantt, dayUnix);
      if (isPausedDay) {
        endDate = GanttMethod.date.datePipe(Gantt, endDate, 1);
      }
      task.startDate = moment(endDate)
        .clone()
        .add(-(task.duration - 1), 'days')
        .format('YYYY-MM-DD');
      const startDate = GanttMethod.date.handleStartDate(
        Gantt,
        task.startDate,
        task.duration,
        endDate
      );
      task.startDate = startDate;
      task.endDate = endDate;
    },
    // 设置工期
    setTaskDuration(Gantt: IXmpp, task: XmppTask, duration: number) {
      let endDate = moment(task.startDate)
        .add(duration - 1, 'days')
        .format('YYYY-MM-DD');
      endDate = GanttMethod.date.handleEndDate(
        Gantt,
        task.startDate,
        duration,
        endDate
      );
      task.duration = duration;
      task.endDate = endDate;
    },
    datePipe(Gantt: IXmpp, date: any, day: number) {
      const getStartDateFromWeekDay = () => {
        const dateUnix = date ? moment(date).unix() : null;
        const dayWeekType = new Date(dateUnix * 1000).getDay();
        if (Gantt.calendar.pauseWeekDayTypes.indexOf(dayWeekType + 1) !== -1) {
          // 时间是周末
          const nextDate = moment(date).clone().add(day, 'days').toDate();
          date = nextDate;
          getStartDateFromWeekDay();
        } else if (Gantt.calendar.exceptDate) {
          // 时间在例外日期之间
          Gantt.calendar.exceptDate.forEach((exceptDate) => {
            const exceptDateStart = moment(exceptDate.fromDate).unix();
            const exceptDateEnd = moment(exceptDate.toDate).unix();
            if (dateUnix <= exceptDateEnd && dateUnix >= exceptDateStart) {
              const nextDate = moment
                .unix(exceptDateEnd)
                .clone()
                .add(day, 'days')
                .toDate();
              date = nextDate;
              getStartDateFromWeekDay();
            }
          });
        }
      };
      const dateUnix = moment(date).unix();
      if (GanttMethod.date.isPausedDay(Gantt, dateUnix)) {
        getStartDateFromWeekDay();
      }
      return date;
    },

    handleStartDate(Gantt: IXmpp, startDate, duration, endDate) {
      if (!duration) {
        duration = 1;
      }
      duration = Number(duration);
      const getCorrectDate = () => {
        const differAndPauseDays = GanttMethod.date.getDifferAndPauseDays(
          Gantt,
          startDate,
          endDate
        );
        const pauseDaysCount = differAndPauseDays.pauseDaysCount;
        const differ = differAndPauseDays.differ;
        if (pauseDaysCount !== 0) {
          if (pauseDaysCount + duration !== differ) {
            startDate = moment(endDate)
              .clone()
              .add(-(pauseDaysCount + duration - 1), 'days')
              .toDate();
            getCorrectDate();
          }
        }
      };
      getCorrectDate();
      return startDate;
    },
    handleEndDate(Gantt: IXmpp, startDate, duration, endDate) {
      if (!duration) {
        duration = 1;
      }
      duration = Number(duration);
      const getCorrectDate = () => {
        const differAndPauseDays = GanttMethod.date.getDifferAndPauseDays(
          Gantt,
          startDate,
          endDate
        );
        const pauseDaysCount = differAndPauseDays.pauseDaysCount;
        const differ = differAndPauseDays.differ;
        if (pauseDaysCount !== 0) {
          if (pauseDaysCount + duration !== differ) {
            endDate = moment(startDate)
              .clone()
              .add(pauseDaysCount + duration - 1, 'days')
              .toDate();
            getCorrectDate();
          }
        }
      };
      getCorrectDate();
      return endDate;
    },

    getDifferAndPauseDays(
      Gantt: IXmpp,
      start,
      end
    ): {
      differ: number;
      pauseDaysCount: number;
    } {
      const startUnix = moment(start).unix();
      const endUnix = moment(end).unix();
      const oneDayLong = 24 * 60 * 60;
      const differ = (endUnix - startUnix) / oneDayLong;
      let pauseDays = 0;
      for (let i = 0; i <= differ; i++) {
        const currentDayUnix = startUnix + i * oneDayLong;
        const currentDayType = new Date(currentDayUnix * 1000).getDay();
        if (
          Gantt.calendar.pauseWeekDayTypes.indexOf(currentDayType + 1) !== -1
        ) {
          pauseDays++;
        }
        if (GanttMethod.date.isExceptDay(Gantt, currentDayUnix)) {
          pauseDays++;
        }
      }
      return {
        differ: differ + 1,
        pauseDaysCount: pauseDays,
      };
    },
    isExceptDay(Gantt: IXmpp, dayUnix) {
      for (const exceptDate of Gantt.calendar.exceptDate) {
        const exceptDateStart = moment(exceptDate.fromDate).unix();
        const exceptDateEnd = moment(exceptDate.toDate).unix();
        // 任务结束时间在例外日期之间
        if (dayUnix <= exceptDateEnd && dayUnix >= exceptDateStart) {
          return true;
        }
      }
      return false;
    },
    isPausedDay(Gantt: IXmpp, dayUnix) {
      for (const exceptDate of Gantt.calendar.exceptDate) {
        const exceptDateStart = moment(exceptDate.fromDate).unix();
        const exceptDateEnd = moment(exceptDate.toDate).unix();
        // 任务结束时间在例外日期之间
        if (dayUnix <= exceptDateEnd && dayUnix >= exceptDateStart) {
          return true;
        }
      }
      const dayWeekType = new Date(dayUnix * 1000).getDay();
      if (Gantt.calendar.pauseWeekDayTypes.indexOf(dayWeekType + 1) !== -1) {
        return true;
      }
    },
  };

  static server = {};
}
