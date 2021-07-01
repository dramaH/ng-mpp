import { Subject } from 'rxjs';
import { GanttMethod } from './gantt.method';
import { XmppColumn, IMPPProject, XmppOptions, XmppTask, IMppTask, IMppExtendAttr, XmppWeekDay, XmppWeekDayType, XmppExceptDate } from './gantt-interface';
import moment, { weekdays, duration } from 'moment';

export class Xmpp {
    allTasks: Array<XmppTask>;
    globalLoading: boolean;
    newLineVisible: boolean;
    mpp: {
        mppInfo: IMPPProject;
        mppTasks: IMppTask[];
        mppExtendAttrs: IMppExtendAttr[];
        extraAttrMap: Map<string, any>;
        /**
         * 处理后台的文件格式
         * @param mppTasks ITask[]
         */
        dealWithMPP(mppProject: IMPPProject, mppTasks: IMppTask[]): void;
        /**
         * 设置MppInfo
         * @param mppProject
         */
        setMppInfo(mppProject: IMPPProject): void;
        setMppTasks(mppTasks: IMppTask[]): void;
        /**
         * 将parentId关系结构处理成wbs层级
         */
        ParentId2WBS(): void;
        /**
         * 查找当前数组中最大的uid
         * @param tragetArray
         */
        findMaxUid(tragetArray: any[]): number;
    };

    task: {
        isFirstInit: boolean;
        isAllSelect: boolean;
        /** 选中任务的颜色 */
        selectedTaskColor: string;
        /** 高亮的任务 */
        activeTaskId: number;
        /** 勾选的所有任务 */
        selectedTasks: XmppTask[];
        /** 任务列表可视任务数量 */
        showTaskLength: number;
        /** canvas绘制参数监听事件 */
        canvasInfoListener: Subject<XmppTask[]>;
        /** 降级折叠后，隐层的任务ids */
        hideTasksIds: [];
        /** 默认的任务列表高度 */
        taskHeight: number;
        /** 任务列表可视区域开始index */
        startTaskIndex: number;
        /** 任务列表可视区域的任务组 */
        showTask: XmppTask[];
        maxUID: number;
        deleteTasksSqlIdStore: string[];
        taskHandleListener: Subject<{ event: MouseEvent, task: XmppTask, XmppInfo: any }>;
        /** 操作后，更新任务关联关系 */
        updateTaskHandle(): void;
        /** 获取折叠之后的可视任务 */
        getAllTaskAfterFold(): XmppTask[];
        /** 升降机操作后，更新任务的层级 */
        updateLeveInfo(): void;
        /** 更新任务时间 */
        updateStartDate(): void;
        /** 更新父任务时间 */
        updateParentStartDate(): void;
        /** 前端删除任务 */
        deleteTaskHandle(taskIds: number[]): void;
        /** 降级任务 */
        depressTaskLevel(tasks: XmppTask[]): void;
        /** 升级任务 */
        promoteTaskLevel(tasks: XmppTask[]): void;
        /** 获取例外日期和时间周后算出来的时间 */
        getStartDateWithExcept(startDate: any, duration: number, endDate: any): any;
        /** 获取例外日期和时间周后算出来的时间 */
        nextDatePipe(startDate: any): any;
        lastDatePipe(endDate: any): any;
        /** 获取例外日期和时间周后算出来的时间 */
        getEndDateWithExcept(startDate: any, duration: number, endDate: any): any;
        /** 更新所有任务id */
        loopAllTasksId(): void;
        /** 添加任务 */
        addTask(): void;
        /** 设置开始时间 */
        setStartDate(task: XmppTask, startDate: string): void;
        /** 设置结束时间 */
        setEndDate(task: XmppTask, endDate: string): void;
        /** 设置工期 */
        setDuration(task: XmppTask, duration: number): void;
    };

    draw: {
        ctxRatio: number;
        baseZoom: number;
        selectedTaskId: number;
        showTooltip: true,
        canvasInfo: any[],
        exceptCanvasInfo: any[],
        canvasHeight: number,
        actualCanvasInfo: any[],
        mouseoverListener: Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>,
        mouseupListener: Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>,
        mousedownListener: Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>,
        contextmenuListener: Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>,
        ctxMask: CanvasRenderingContext2D;
        color: {
            /** 计划时间颜色 */
            planColor: string,
            /** 任务为关键线路时，计划时间颜色 */
            planKeyColor: string,
            /** 实际时间颜色 */
            Actualcolor: string,
            /** 任务为关键线路时，实际时间颜色 */
            ActualkeyColor: string,
            /** 箭头及连线颜色 */
            arrowColor: string,
            /** 任务为关键线路时，箭头及连线颜色 */
            arrowKeyColor: string,
            /** 例外日期区域的颜色 */
            exceptDateColor: string
        },
        lineHeight: number,
        actualLineHeight: number,
        canvasWidth: number,
        canvasLeftHide: number,
        setCanvasWidth: (width: number) => void,
        updateCanvasInfo: () => void,
        drawExceptArea: (ctx: CanvasRenderingContext2D) => void,
        drawTasks: (ctx: CanvasRenderingContext2D, isActual?: boolean) => void,
        drawSelectTask: (taskId: number) => void,
        canvasMouseEventListen: () => void,
        drawTooltip: (ele, task: XmppTask) => void,
        zoom: (number) => void

    };

    column: {
        columnNames: any[],
        totalWidth: number,
        setColumn: (param: XmppColumn[]) => void;
    };

    calendar: {
        /** 默认日历id */
        calendarId: string,
        /** 日历周数组 */
        weeksArry: any[],
        /** 日历总宽度 */
        calenderWidth: number,
        /** 单元格日历（天）的宽度 */
        baseCellWidth: number,
        /** 例外日期 */
        exceptDate: XmppExceptDate[],
        /** 处理例外日期 */
        // dealWithExceptDate: () => void,
        /** 工作周 */
        weekDays: XmppWeekDay[];
        pauseWeekDays: string[];
        /** 工作周休息日期枚举数组：[1, 7]即休息周六周日 */
        pauseWeekDayTypes: number[];
        minLineDay: any;
        maxLineDay: any;
    };
    render: () => void;
    addGanttEventListener: (type: 'caculateListener' | 'mouseoverListener' | 'mouseupListener' | 'mousedownListener' | 'contextmenuListener', cb?: (res: { event: MouseEvent; task: XmppTask; position: any }) => void) => void;
    addTaskEventListener: (type: 'clickListener' | 'dblclickListener', cb?: (res: { event: MouseEvent; task: XmppTask; XmppInfo: any }) => void) => void;
    constructor() {
        this.globalLoading = false;
        const currentMpp = this;
        this.task = {
            selectedTaskColor: 'rgba(65,159,229, 1)',
            isFirstInit: false,
            isAllSelect: false,
            activeTaskId: null,
            selectedTasks: [],
            /** 任务列表可视任务数量 */
            showTaskLength: 0,
            /** 日历绘制监听事件 */
            canvasInfoListener: new Subject<XmppTask[]>(),
            /** 降级折叠后，隐层的任务ids */
            hideTasksIds: [],
            /** 默认的任务列表高度 */
            taskHeight: 36,
            /** 任务列表可视区域开始index */
            startTaskIndex: 0,
            /** 任务列表可视区域的任务组 */
            showTask: [],
            maxUID: 0,
            deleteTasksSqlIdStore: [],
            taskHandleListener: new Subject<{ event: MouseEvent, task: XmppTask, XmppInfo: any }>(),
            /** 操作后，更新任务关联关系 */
            updateTaskHandle() {
                GanttMethod.tasks.updateTaskHandle(currentMpp);
            },
            /** 获取折叠之后的可视任务 */
            getAllTaskAfterFold() {
                return GanttMethod.tasks.getAllTaskAfterFold(currentMpp);
            },
            /** 升降机操作后，更新任务的层级 */
            updateLeveInfo() {
                GanttMethod.tasks.updateLeveInfo(currentMpp.allTasks);
            },
            /** 更新任务时间 */
            updateStartDate() {
                GanttMethod.date.updateStartDate(currentMpp);
            },
            /** 更新父任务时间 */
            updateParentStartDate() {
                GanttMethod.date.updateParentStartDate(currentMpp.allTasks);
            },
            /** 前端添加任务 */
            // addTaskHandle(taskParam: any) {
            //     GanttMethod.tasks.addTaskHandle(currentMpp, taskParam);
            // },
            addTask() {
                GanttMethod.tasks.addTask(currentMpp);
            },
            /** 前端删除任务 */
            deleteTaskHandle(taskIds: number[]) {
                GanttMethod.tasks.deleteTaskHandle(currentMpp, taskIds);
            },
            /** 降级任务 */
            depressTaskLevel(tasks: XmppTask[]) {
                GanttMethod.tasks.depressTaskLevel(currentMpp, tasks);
            },
            /** 升级任务 */
            promoteTaskLevel(tasks: XmppTask[]) {
                GanttMethod.tasks.promoteTaskLevel(currentMpp, tasks);
            },
            nextDatePipe(startDate: any) {
                return GanttMethod.date.datePipe(currentMpp, startDate, 1);
            },
            lastDatePipe(startDate: any) {
                return GanttMethod.date.datePipe(currentMpp, startDate, -1);
            },
            getStartDateWithExcept(startDate: any, duration: number, endDate: any) {
                return GanttMethod.date.handleStartDate(currentMpp, startDate, duration, endDate);
            },
            getEndDateWithExcept(startDate: any, duration: number, endDate: any) {
                return GanttMethod.date.handleEndDate(currentMpp, startDate, duration, endDate);
            },
            loopAllTasksId() {
                for (let i = 0; i < currentMpp.allTasks.length; i++) {
                    const element = currentMpp.allTasks[i];
                    element.id = i + 1;
                }
                currentMpp.render();
            },
            setStartDate(task: XmppTask, startDate: string) {
                return GanttMethod.date.setTaskStartDate(currentMpp, task, startDate);
            },
            setEndDate(task: XmppTask, endDate: string) {
                return GanttMethod.date.setTaskEndDate(currentMpp, task, endDate);
            },
            setDuration(task: XmppTask, duration: number) {
                return GanttMethod.date.setTaskDuration(currentMpp, task, duration);
            },

        };
        this.mpp = {
            mppInfo: null,
            mppTasks: [],
            mppExtendAttrs: [],
            extraAttrMap: new Map(),
            /**
             * 处理后台的文件格式
             * @param mppTasks ITask[]
             */
            dealWithMPP(mppProject: IMPPProject, mppTasks: IMppTask[]) {
                currentMpp.task.showTask = [];

            },
            setMppInfo(mppProject: IMPPProject) {
                GanttMethod.mpp.dealWithProject(currentMpp, mppProject);
            },

            setMppTasks(mppTasks: IMppTask[]) {
                const allTasks = GanttMethod.mpp.dealWithMPPTasks(currentMpp, mppTasks);
                currentMpp.allTasks = allTasks;
            },
            /**
             * 将parentId关系结构处理成wbs层级
             */
            ParentId2WBS() {
                GanttMethod.mpp.ParentId2WBS(currentMpp);
            },
            findMaxUid(tragetArray: any[]) {
                return GanttMethod.mpp.findMaxUid(tragetArray);
            }
        };

        this.draw = {
            ctxRatio: 2,
            baseZoom: 1,
            selectedTaskId: null,
            showTooltip: true,
            canvasInfo: [],
            exceptCanvasInfo: [],
            canvasHeight: 0,
            actualCanvasInfo: [],
            mouseoverListener: new Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>(),
            mouseupListener: new Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>(),
            mousedownListener: new Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>(),
            contextmenuListener: new Subject<{ event: MouseEvent, task: XmppTask, position: { x: number, y: number } }>(),
            ctxMask: null,
            color: {
                /** 计划时间颜色 */
                planColor: 'rgba(65,159,229, 0.2)',
                /** 任务为关键线路时，计划时间颜色 */
                planKeyColor: 'rgba(255, 128, 128, 0.2)',
                /** 实际时间颜色 */
                Actualcolor: '#419fe8',
                /** 任务为关键线路时，实际时间颜色 */
                ActualkeyColor: '#ff8080',
                /** 箭头及连线颜色 */
                arrowColor: 'rgba(65,159,229, 0.2)',
                /** 任务为关键线路时，箭头及连线颜色 */
                arrowKeyColor: 'rgba(255, 128, 128, 0.2)',
                exceptDateColor: '#ff8080'
            },
            lineHeight: 26,
            actualLineHeight: 10,
            canvasWidth: 0,
            setCanvasWidth(width) {
                if (!currentMpp.draw.canvasWidth || currentMpp.draw.canvasWidth !== width) {
                    currentMpp.draw.canvasWidth = width;
                    currentMpp.task.canvasInfoListener.next();
                }
            },
            canvasLeftHide: 0,
            updateCanvasInfo() {
                GanttMethod.canvas.updateCanvasInfo(currentMpp);
                currentMpp.task.canvasInfoListener.next();
            },
            drawExceptArea(ctx: CanvasRenderingContext2D) {
                GanttMethod.canvas.drawExceptArea(currentMpp, ctx);
            },
            drawTasks(ctx: CanvasRenderingContext2D, isActual?: boolean) {
                GanttMethod.canvas.drawTasks(currentMpp, ctx, isActual);
            },
            drawSelectTask(taskId: number) {
                GanttMethod.canvas.drawSelectTask(currentMpp, taskId);
            },
            canvasMouseEventListen() {
                const windowToCanvas = (canvas, x, y) => {
                    const rect = canvas.getBoundingClientRect();
                    return {
                        x: x - (rect.left * (canvas.width / rect.width)) / 2,
                        y: y - (rect.top * (canvas.height / rect.height)) / 2
                    };
                };
                const canvas = document.getElementById('maskCanvas');
                const isTask = (ele) => {
                    const planIndex = currentMpp.draw.canvasInfo.findIndex(item => {
                        return ele.x > item.positionX && ele.x < item.positionX + item.width &&
                            ele.y > item.positionY && ele.y < item.positionY + item.lineHeight;
                    });
                    const actualIndex = currentMpp.draw.actualCanvasInfo.findIndex(item => {
                        return ele.x > item.positionX && ele.x < item.positionX + item.width &&
                            ele.y > item.positionY && ele.y < item.positionY + item.lineHeight;
                    });
                    let task: XmppTask;
                    if (planIndex !== -1) {
                        task = currentMpp.allTasks[planIndex];
                    }
                    if (actualIndex !== -1) {
                        task = currentMpp.allTasks[actualIndex];
                    }
                    return task;
                };
                canvas.onmousemove = (e: MouseEvent) => {
                    const ele = windowToCanvas(canvas, e.clientX, e.clientY);

                    const task = isTask(ele);
                    currentMpp.draw.mouseoverListener.next({ event: e, task, position: ele });
                    if (currentMpp.draw.showTooltip) {
                        currentMpp.draw.drawTooltip(ele, task);
                        if (currentMpp.draw.selectedTaskId) {
                            currentMpp.draw.drawSelectTask(currentMpp.draw.selectedTaskId);
                        }
                    }
                };
                canvas.onmouseup = (e) => {
                    const ele = windowToCanvas(canvas, e.clientX, e.clientY);
                    const task = isTask(ele);
                    currentMpp.draw.mouseupListener.next({ event: e, task, position: ele });
                };
                canvas.onmousedown = (e) => {
                    const ele = windowToCanvas(canvas, e.clientX, e.clientY);
                    const task = isTask(ele);
                    currentMpp.draw.mousedownListener.next({ event: e, task, position: ele });
                };
                canvas.oncontextmenu = (e) => {
                    const ele = windowToCanvas(canvas, e.clientX, e.clientY);
                    const task = isTask(ele);
                    currentMpp.draw.contextmenuListener.next({ event: e, task, position: ele });
                };
            },

            drawTooltip(ele, task: XmppTask) {
                // const c = (document.getElementById('maskCanvas')) as HTMLCanvasElement;
                const ctxMask = currentMpp.draw.ctxMask;
                const panelWidth = document.getElementById('maskCanvas').clientWidth;
                const tipWidth = 180;
                let tipX = ele.x + 10;
                const tipY = ele.y + 5;
                if (tipX + tipWidth > panelWidth) {
                    tipX = ele.x - 180;
                }
                if (task) {
                    let style = document.getElementById('maskCanvas').getAttribute('style');
                    style = style + ';cursor: pointer';
                    document.getElementById('maskCanvas').setAttribute('style', style);
                    ctxMask.clearRect(0, 0, panelWidth * currentMpp.draw.ctxRatio, currentMpp.draw.canvasHeight * currentMpp.draw.ctxRatio);
                    ctxMask.fillStyle = 'rgba(0,0,0,0.6)';
                    GanttMethod.canvas.drawRoundRect(ctxMask, tipX * currentMpp.draw.ctxRatio, tipY * currentMpp.draw.ctxRatio, tipWidth * currentMpp.draw.ctxRatio, 60 * currentMpp.draw.ctxRatio, 5);
                    ctxMask.fill();
                    ctxMask.fillStyle = '#fff';
                    const planText = task.startDate ?
                        moment(task.startDate).format('YYYY.MM.DD') + '-' + moment(task.endDate).format('YYYY.MM.DD') :
                        '暂无';
                    // const actualText = task.actualStartDate ?
                    //     '实际时间：' + moment(task.actualStartDate).format('YYYY.MM.DD') + '-' + moment(task.actualEndDate).format('YYYY.MM.DD') :
                    //     '实际时间：暂无';
                    const nameText = task.taskName;
                    GanttMethod.canvas.drawText(ctxMask, nameText, (tipX + 15) * currentMpp.draw.ctxRatio, (tipY + 23) * currentMpp.draw.ctxRatio, 12 * currentMpp.draw.ctxRatio, '#fff');
                    GanttMethod.canvas.drawText(ctxMask, planText, (tipX + 15) * currentMpp.draw.ctxRatio, (tipY + 42) * currentMpp.draw.ctxRatio, 12 * currentMpp.draw.ctxRatio, '#fff');
                } else {
                    // document.getElementById('maskCanvas').setAttribute('style', `cursor: default`);
                    ctxMask.clearRect(0, 0, panelWidth * currentMpp.draw.ctxRatio, currentMpp.draw.canvasHeight * currentMpp.draw.ctxRatio);
                }

            },
            zoom(zoom) {
                currentMpp.draw.baseZoom = zoom;
                currentMpp.task.canvasInfoListener.next();
            }

        };

        this.column = {
            columnNames: [],
            totalWidth: 0,
            setColumn: (param: XmppColumn[]) => {
                currentMpp.column.totalWidth = 46;
                for (const cv of param) {
                    if (!cv.type) {
                        cv.type = 'text';
                    }
                    let resizeBarWidth = 0;
                    if (cv.resize) {
                        resizeBarWidth = 1;
                    }
                    currentMpp.column.totalWidth = currentMpp.column.totalWidth + cv.width + resizeBarWidth;
                }
                currentMpp.column.columnNames = param;
            }
        };

        this.calendar = {
            calendarId: null,
            /** 日历周数组 */
            weeksArry: [],
            /** 日历总宽度 */
            calenderWidth: 0,
            /** 单元格日历（天）的宽度 */
            baseCellWidth: 25,
            /** 例外日期 */
            exceptDate: [],
            /** 处理例外日期 */
            // dealWithExceptDate() {
            //     GanttMethod.date.dealWithExceptDate(currentMpp);
            // },
            /** 工作周 */
            weekDays: [],
            // 日历中放假的所有日期
            pauseWeekDays: [],
            pauseWeekDayTypes: [],
            maxLineDay: '',
            minLineDay: ''
        };

        this.render = () => {
            // 清空canvas
            currentMpp.draw.canvasInfo = [];
            currentMpp.draw.actualCanvasInfo = [];
            if (currentMpp.allTasks) {

                // 升级降级
                currentMpp.task.updateLeveInfo();

                // 根据前置任务的时间更新startDate
                currentMpp.task.updateStartDate();
                // 更新parent的startDate
                currentMpp.task.updateParentStartDate();

                // currentMpp.calendar.dealWithExceptDate();


                // // 更新实际startDate
                // this.updateActualStartDate();
                // // 更新parent的actualStartDate
                // this.updateParentActualStartDate();

                // this.dealWithActualExceptDate()

                // 计算showTask
                currentMpp.task.updateTaskHandle();
                // 最后更新 canvas和日历(update canvas)
                currentMpp.draw.updateCanvasInfo();
                // 画布鼠标事件的监听
                currentMpp.draw.canvasMouseEventListen();

            } else {
                // this.showTask = [];
                currentMpp.calendar.weeksArry = [];
            }

        };

        /**
         * canvas区域任务的鼠标事件
         */
        this.addGanttEventListener = (
            type: 'caculateListener' | 'mouseoverListener' | 'mouseupListener' | 'mousedownListener' | 'contextmenuListener',
            cb?: (res: { event: MouseEvent, task: XmppTask, position: { x: number, y: number } }) => void) => {
            if (type === 'mouseoverListener') {
                currentMpp.draw.mouseoverListener.subscribe(({ event: e, task, position: ele }) => {
                    cb({ event: e, task, position: ele });
                });
            }
            if (type === 'mouseupListener') {
                currentMpp.draw.mouseupListener.subscribe(({ event: e, task, position: ele }) => {
                    cb({ event: e, task, position: ele });
                });
            }
            if (type === 'mousedownListener') {
                currentMpp.draw.mousedownListener.subscribe(({ event: e, task, position: ele }) => {
                    cb({ event: e, task, position: ele });
                });
            }

            if (type === 'contextmenuListener') {
                currentMpp.draw.contextmenuListener.subscribe(({ event: e, task, position: ele }) => {
                    cb({ event: e, task, position: ele });
                });
            }
        };

        /**
         * 任务的鼠标事件
         */
        this.addTaskEventListener = (
            type: 'clickListener' | 'dblclickListener',
            cb?: (res: { event: MouseEvent, task: XmppTask, XmppInfo: any }) => void) => {
            currentMpp.task.taskHandleListener.subscribe(({ event: e, task, XmppInfo: ele }) => {
                if (type === 'clickListener' && e.type === 'click') {
                    cb({ event: e, task, XmppInfo: ele });
                }
                if (type === 'dblclickListener' && e.type === 'dblclick') {
                    cb({ event: e, task, XmppInfo: ele });
                }
            });
        };

    }


}

// @dynamic
// export class Gantt {
//     static currentGantt: IMPPProject;
//     static allTasks: Array<GanttModel> = [];
//     static extraInfo: any;
//     static currentTask: GanttModel;
//     static mpp = {
//         extraAttrMap: new Map(),
//         /**
//          * 处理后台的文件格式
//          * @param mppTasks ITask[]
//          */
//         dealWithMPP(mppProject: IMPPProject, mppTasks: ITask[]) {
//             Gantt.task.showTask = [];
//             const allTasks = GanttMethod.mpp.dealWithMPP(Gantt, mppTasks);
//             GanttMethod.mpp.dealWithProject(Gantt, mppProject);
//             Gantt.allTasks = allTasks;
//         },
//         /**
//          * 将parentId关系结构处理成wbs层级
//          */
//         ParentId2WBS() {
//             GanttMethod.mpp.ParentId2WBS(Gantt);
//         }
//     };
//     static task = {
//         /** 任务列表可视任务数量 */
//         showTaskLength: 0,
//         /** 日历绘制监听事件 */
//         caculateListener: new Subject<GanttModel[]>(),
//         /** 降级折叠后，隐层的任务ids */
//         hideTasksIds: [],
//         /** 默认的任务列表高度 */
//         taskHeight: 36,
//         /** 任务列表可视区域开始index */
//         startTaskIndex: 0,
//         /** 任务列表可视区域的任务组 */
//         showTask: [],
//         maxUID: 0,
//         /** 操作后，更新任务关联关系 */
//         updateTaskHandle() {
//             GanttMethod.tasks.updateTaskHandle(Gantt);
//         },
//         /** 获取折叠之后的可视任务 */
//         getAllTaskAfterFold() {
//             return GanttMethod.tasks.getAllTaskAfterFold(Gantt);
//         },
//         /** 升降机操作后，更新任务的层级 */
//         updateLeveInfo() {
//             GanttMethod.tasks.updateLeveInfo(Gantt.allTasks);
//         },
//         /** 更新任务时间 */
//         updateStartDate() {
//             GanttMethod.date.updateStartDate(Gantt);
//         },
//         /** 更新父任务时间 */
//         updateParentStartDate() {
//             GanttMethod.date.updateParentStartDate(Gantt.allTasks);
//         },
//         /** 前端添加任务 */
//         addTaskHandle(task?: IAddTask) {
//             GanttMethod.tasks.addTaskHandle(Gantt, task);
//         },
//         /** 前端删除任务 */
//         deleteTaskHandle(taskIds: number[]) {
//             GanttMethod.tasks.deleteTaskHandle(Gantt, taskIds);
//         },
//         /** 降级任务 */
//         depressTaskLevel(tasks: GanttModel[]) {
//             GanttMethod.tasks.depressTaskLevel(Gantt, tasks);
//         },
//         /** 升级任务 */
//         promoteTaskLevel(tasks: GanttModel[]) {
//             GanttMethod.tasks.promoteTaskLevel(Gantt, tasks);
//         }
//     };

//     static calendar = {
//         /** 日历周数组 */
//         weeksArry: [],
//         /** 日历总宽度 */
//         calenderWidth: 0,
//         /** 单元格日历（天）的宽度 */
//         baseCellWidth: 25,
//         /** 例外日期 */
//         exceptDate: [],
//         /** 处理例外日期 */
//         dealWithExceptDate() {
//             GanttMethod.date.dealWithExceptDate(Gantt);
//         }
//     };

//     static draw = {
//         showTooltip: true,
//         canvasInfo: [],
//         exceptCanvasInfo: [],
//         canvasHeight: 0,
//         actualCanvasInfo: [],
//         mouseoverListener: new Subject<{ event: MouseEvent, task: GanttModel, position: { x: number, y: number } }>(),
//         mouseupListener: new Subject<{ event: MouseEvent, task: GanttModel, position: { x: number, y: number } }>(),
//         mousedownListener: new Subject<{ event: MouseEvent, task: GanttModel, position: { x: number, y: number } }>(),
//         contextmenuListener: new Subject<{ event: MouseEvent, task: GanttModel, position: { x: number, y: number } }>(),
//         color: {
//             /** 计划时间颜色 */
//             planColor: 'rgba(65,159,229, 0.2)',
//             /** 任务为关键线路时，计划时间颜色 */
//             planKeyColor: 'rgba(255, 128, 128, 0.2)',
//             /** 任务为延期任务时，计划时间颜色 */
//             planDelayColor: 'rgba(65,159,229, 0.2)',
//             /** 实际时间颜色 */
//             Actualcolor: '#419fe8',
//             /** 任务为关键线路时，实际时间颜色 */
//             ActualkeyColor: '#ff8080',
//             /** 任务为延期任务时，实际时间颜色 */
//             ActualDelayColor: '#419fe8',
//             /** 箭头及连线颜色 */
//             arrowColor: 'rgba(65,159,229, 0.2)',
//             /** 任务为关键线路时，箭头及连线颜色 */
//             arrowKeyColor: 'rgba(255, 128, 128, 0.2)'
//         },
//         lineHeight: 26,
//         actualLineHeight: 10,
//         canvasWidth: 0,
//         setCanvasWidth(width) {
//             if (!Gantt.draw.canvasWidth || Gantt.draw.canvasWidth !== width) {
//                 Gantt.draw.canvasWidth = width;
//                 Gantt.task.caculateListener.next();
//             }
//         },
//         canvasLeftHide: 0,
//         updateCanvasInfo() {
//             GanttMethod.canvas.updateCanvasInfo(Gantt);
//             Gantt.task.caculateListener.next();
//         },
//         drawExceptArea(ctx: CanvasRenderingContext2D) {
//             GanttMethod.canvas.drawExceptArea(Gantt, ctx);
//         },
//         drawTasks(ctx: CanvasRenderingContext2D, isActual?: boolean) {
//             GanttMethod.canvas.drawTasks(Gantt, ctx, isActual);
//         },
//         canvasMouseEventListen() {
//             const windowToCanvas = (canvas, x, y) => {
//                 const rect = canvas.getBoundingClientRect();
//                 return {
//                     x: x - rect.left * (canvas.width / rect.width),
//                     y: y - rect.top * (canvas.height / rect.height)
//                 };
//             };
//             const canvas = document.getElementById('maskCanvas');
//             const isTask = (ele) => {
//                 const planIndex = Gantt.draw.canvasInfo.findIndex(item => {
//                     return ele.x > item.positionX && ele.x < item.positionX + item.width &&
//                         ele.y > item.positionY && ele.y < item.positionY + item.lineHeight;
//                 });
//                 const actualIndex = Gantt.draw.actualCanvasInfo.findIndex(item => {
//                     return ele.x > item.positionX && ele.x < item.positionX + item.width &&
//                         ele.y > item.positionY && ele.y < item.positionY + item.lineHeight;
//                 });
//                 let task: GanttModel;
//                 if (planIndex !== -1) {
//                     task = Gantt.allTasks[planIndex];
//                 }
//                 if (actualIndex !== -1) {
//                     task = Gantt.allTasks[actualIndex];
//                 }
//                 return task;
//             };
//             canvas.onmousemove = (e: MouseEvent) => {
//                 const ele = windowToCanvas(canvas, e.clientX, e.clientY);
//                 const task = isTask(ele);
//                 Gantt.draw.mouseoverListener.next({ event: e, task, position: ele });
//                 if (Gantt.draw.showTooltip) {
//                     Gantt.draw.drawTooltip(ele, task);
//                 }
//             };
//             canvas.onmouseup = (e) => {
//                 const ele = windowToCanvas(canvas, e.clientX, e.clientY);
//                 const task = isTask(ele);
//                 Gantt.draw.mouseupListener.next({ event: e, task, position: ele });
//             };
//             canvas.onmousedown = (e) => {
//                 const ele = windowToCanvas(canvas, e.clientX, e.clientY);
//                 const task = isTask(ele);
//                 Gantt.draw.mousedownListener.next({ event: e, task, position: ele });
//             };
//             canvas.oncontextmenu = (e) => {
//                 const ele = windowToCanvas(canvas, e.clientX, e.clientY);
//                 const task = isTask(ele);
//                 Gantt.draw.contextmenuListener.next({ event: e, task, position: ele });
//             };
//         },

//         drawTooltip(ele, task: GanttModel) {
//             const c = (document.getElementById('maskCanvas')) as HTMLCanvasElement;
//             const ctxMask = c.getContext('2d');
//             const panelWidth = document.getElementById('maskCanvas').clientWidth;
//             const tipWidth = 220;
//             let tipX = ele.x + 10;
//             const tipY = ele.y + 5;
//             if (tipX + tipWidth > panelWidth) {
//                 tipX = ele.x - 220;
//             }
//             if (task) {
//                 document.getElementById('maskCanvas').setAttribute('style', `cursor: pointer`);
//                 ctxMask.clearRect(0, 0, panelWidth, Gantt.draw.canvasHeight);
//                 ctxMask.fillStyle = 'rgba(0,0,0,0.6)';
//                 GanttMethod.canvas.drawRoundRect(ctxMask, tipX, tipY, tipWidth, 60, 5);
//                 ctxMask.fill();
//                 ctxMask.fillStyle = '#fff';
//                 const planText = task.startDate ?
//                     '计划时间：' + moment(task.startDate).format('YYYY.MM.DD') + '-' + moment(task.endDate).format('YYYY.MM.DD') :
//                     '计划时间：暂无';
//                 const actualText = task.actualStartDate ?
//                     '实际时间：' + moment(task.actualStartDate).format('YYYY.MM.DD') + '-' + moment(task.actualEndDate).format('YYYY.MM.DD') :
//                     '实际时间：暂无';
//                 GanttMethod.canvas.drawText(ctxMask, planText, tipX + 15, tipY + 23, 12);
//                 GanttMethod.canvas.drawText(ctxMask, actualText, tipX + 15, tipY + 42, 12);
//             } else {
//                 document.getElementById('maskCanvas').setAttribute('style', `cursor: default`);
//                 ctxMask.clearRect(0, 0, panelWidth, Gantt.draw.canvasHeight);
//             }

//         }

//     };



//     static column = {
//         columnNames: [],
//         totalWidth: 0,
//         setColumn: (param: IColumn[]) => {
//             for (const cv of param) {
//                 if (!cv.type) {
//                     cv.type = 'text';
//                 }
//                 Gantt.column.totalWidth = Gantt.column.totalWidth + cv.width + 5;
//             }
//             Gantt.column.columnNames = param;
//         }
//     };

//     static render() {
//         // 清空canvas
//         Gantt.draw.canvasInfo = [];
//         Gantt.draw.actualCanvasInfo = [];
//         if (Gantt.allTasks.length > 0) {

//             // 升级降级
//             Gantt.task.updateLeveInfo();

//             // 根据前置任务的时间更新startDate
//             Gantt.task.updateStartDate();
//             // 更新parent的startDate
//             Gantt.task.updateParentStartDate();
//             // 计算showDuration
//             Gantt.calendar.dealWithExceptDate();


//             // // 更新实际startDate
//             // this.updateActualStartDate();
//             // // 更新parent的actualStartDate
//             // this.updateParentActualStartDate();
//             // // 计算showDuration
//             // this.dealWithActualExceptDate()

//             // 计算showTask
//             Gantt.task.updateTaskHandle();
//             // 最后更新 canvas和日历(update canvas)
//             Gantt.draw.updateCanvasInfo();
//             // 画布鼠标事件的监听
//             Gantt.draw.canvasMouseEventListen();

//         } else {
//             // this.showTask = [];
//             Gantt.calendar.weeksArry = [];
//         }

//     }

//     /**
//      * canvas区域任务的鼠标事件
//      */
//     static addGanttEventListener = (
//         type: 'caculateListener' | 'mouseoverListener' | 'mouseupListener' | 'mousedownListener' | 'contextmenuListener',
//         cb?: (res: { event: MouseEvent, task: GanttModel, position: { x: number, y: number } }) => void) => {
//         if (type === 'mouseoverListener') {
//             Gantt.draw.mouseoverListener.subscribe(({ event: e, task, position: ele }) => {
//                 cb({ event: e, task, position: ele });
//             });
//         }
//         if (type === 'mouseupListener') {
//             Gantt.draw.mouseupListener.subscribe(({ event: e, task, position: ele }) => {
//                 cb({ event: e, task, position: ele });
//             });
//         }
//         if (type === 'mousedownListener') {
//             Gantt.draw.mousedownListener.subscribe(({ event: e, task, position: ele }) => {
//                 cb({ event: e, task, position: ele });
//             });
//         }

//         if (type === 'contextmenuListener') {
//             Gantt.draw.contextmenuListener.subscribe(({ event: e, task, position: ele }) => {
//                 cb({ event: e, task, position: ele });
//             });
//         }
//     }

//     // static GanttTest() {
//     //     GanttMethod.canvas.test(Gantt);
//     // }
// }


