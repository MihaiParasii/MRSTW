import { Component, inject, OnInit } from '@angular/core'
import { DealService } from '../../../services/deal/deal.service'
import { DealResponse } from '../../../models/deal/deal-response'

@Component({
	selector: 'app-deals-page',
	imports: [],
	templateUrl: './deals-page.component.html',
	styleUrl: './deals-page.component.css'
})
export class DealsPageComponent implements OnInit {
	private dealService: DealService = inject(DealService)
	deals: DealResponse[] = []
	
	
	ngOnInit(): void {
		this.dealService.get().subscribe(deals => {
          this.deals = deals
        })
	}
}
