import {Component, inject} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {DealResponse} from '../../../models/deal/deal-response';
import {DealService} from '../../../services/deal/deal.service';
import {DealCardComponent} from '../deal-card/deal-card.component';

@Component({
  selector: 'app-search-page',
  imports: [
    DealCardComponent
  ],
  templateUrl: './search-page.component.html',
  standalone: true,
  styleUrl: './search-page.component.css'
})
export class SearchPageComponent {
  private dealService: DealService = inject(DealService);
  private query!: string;
  protected deals: DealResponse[] = [];

  constructor(private route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.query = params['query'];
    });


    this.dealService.get().subscribe(value => {
      this.deals = value;
    })
  }
}
