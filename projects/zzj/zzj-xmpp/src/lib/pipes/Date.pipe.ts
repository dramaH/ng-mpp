import { Pipe, PipeTransform } from '@angular/core';
import moment from 'moment';

@Pipe({
  name: 'datePipe'
})
export class DatePipe implements PipeTransform {

  transform(value: any, ...args: any[]): any {
    let result = '';
    if (value && value !== '') {
      result = moment(value).format('YYYY-MM-DD');
    }
    return result;
  }

}
