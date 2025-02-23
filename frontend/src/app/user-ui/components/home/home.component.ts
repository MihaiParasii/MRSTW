import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {DealResponse} from '../../../models/deal/deal-response';
import {DealCardComponent} from '../deal-card/deal-card.component';
import {
  SidebarCategoriesComponent
} from '../../../common-ui/components/sidebar-categories/sidebar-categories.component';

@Component({
  selector: 'app-home',
  imports: [
    RouterOutlet,
    SidebarCategoriesComponent,
    DealCardComponent,
  ],
  templateUrl: './home.component.html',
  standalone: true,
  styleUrl: './home.component.css'
})
export class HomeComponent {
  protected deals: Array<DealResponse> = [];

  constructor() {
    // init with 9 deals
    let deal = new DealResponse();

    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
    this.deals.push(deal);
  }
}
