import { isNullOrUndefined } from 'util';
import moment from 'moment';

export const GanttSize = {
    // 底部编辑框高度
    bottomHeight: 300,
    // 每条任务的高度
    taskHeight: 36,
    // 日历一格的宽度
    calenderBaseWidth: 25
};

export class GanttProjectModel {
    public id: string;
    public parentId: string;
    public calendars: any = [];
    public creationDate: string;
    public title: string;
    public company: string;
    public author: string;
    public startDate: any;
    public finishDate: any;
    public dateFormat: any = [];
    public constructor(param) {
        (!isNullOrUndefined(param.id)) && (this.id = param.id);
        (!isNullOrUndefined(param.parentId)) && (this.parentId = param.parentId);
        (!isNullOrUndefined(param.calendars)) && (this.calendars = param.calendars);
        (!isNullOrUndefined(param.creationDate)) && (this.creationDate = param.creationDate);
        (!isNullOrUndefined(param.title)) && (this.title = param.title);
        (!isNullOrUndefined(param.company)) && (this.company = param.company);
        (!isNullOrUndefined(param.author)) && (this.author = param.author);
        (!isNullOrUndefined(param.startDate)) && (this.startDate = param.startDate);
        (!isNullOrUndefined(param.finishDate)) && (this.finishDate = param.finishDate);
        (!isNullOrUndefined(param.dateFormat)) && (this.dateFormat = param.dateFormat);
    }

    public toCreateJson() {
        return {
            Title: this.title,
            Company: this.company,
            Author: this.author,
            StartDate: moment(this.dateFormat[0]).toJSON(),
            FinishDate: moment(this.dateFormat[1]).toJSON()
        };
    }
}


