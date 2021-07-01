import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-explain',
  templateUrl: './explain.component.html',
  styleUrls: ['./explain.component.less']
})
export class ExplainComponent implements OnInit {
  routeType: string = '';
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {
    let that = this;
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(() => this.router)
      )
      .subscribe((event) => {
        let type = this.activatedRoute.snapshot.paramMap.get('type');
        if (that.routeType != type) {

          that.routeType = type;
        }

        // console.log(this.routeType)
      });
  }

  ngOnInit() {
    console.log('explain')
  }

}
