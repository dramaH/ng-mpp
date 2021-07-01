import { Pipe, PipeTransform } from '@angular/core';
import { XmppPredecessorLink } from '../gantt-interface';
import { prevType2Str } from '../gantt.config';
@Pipe({
    name: 'relation'
})
export class RelationPipe implements PipeTransform {

    transform(prevRelation: XmppPredecessorLink[], ...args: any[]): any {
        console.log(prevRelation)
        let str = ''
        if (prevRelation.length == 0) {
            return str;
        }
        prevRelation.forEach(pr => {
            str = str + pr.prevId + prevType2Str[pr.relation]
        })
        return str;
    }

}