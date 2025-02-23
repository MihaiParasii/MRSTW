import {Component} from '@angular/core';
import {DealService} from '../../../services/deal/deal.service';
import {DealResponse} from '../../../models/deal/deal-response';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-deal-detail-page',
  imports: [],
  templateUrl: './deal-detail-page.component.html',
  standalone: true,
  styleUrl: './deal-detail-page.component.css'
})
export class DealDetailPageComponent {
  protected deal!: DealResponse;

  constructor(private dealService: DealService, private route: ActivatedRoute) {
    let strId: string | null = "";

    this.route.paramMap.subscribe(params => {
      strId = params.get("id");
    })

    if (strId) {
      this.dealService.getById(Number(strId)).subscribe(value => {
        this.deal = value;
      })
    }
  }
}
