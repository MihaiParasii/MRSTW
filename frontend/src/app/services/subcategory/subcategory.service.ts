import { inject, Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { SubcategoryResponse } from '../../models/subcategory/subcategory-response'
import { CreateSubcategoryRequest } from '../../models/subcategory/create-subcategory-request'
import { UpdateSubcategoryRequest } from '../../models/subcategory/update-subcategory-request'

@Injectable({
	providedIn: 'root'
})
export class SubcategoryService {
	http = inject(HttpClient)
	
	url = 'http://localhost:8080/api/Subcategory/v1'
	
	
	get(): Observable<SubcategoryResponse[]> {
		const url = this.url
		return this.http.get<SubcategoryResponse[]>(url)
	}
	
	getById(id: number): Observable<SubcategoryResponse> {
		return this.http.get<SubcategoryResponse>(`${this.url}/${id}`)
	}
	
	getByCategoryId(id: number) {
		// return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)
	}
	
	create(request: CreateSubcategoryRequest) {
		// return this.http.post(`${this.testApiUrl}/doctor`, {
		//   "Name": request.Name,
		//   "Surname": request.Surname,
		//   "PhotoPath": request.PhotoPath,
		//   "SpecialityId": request.SpecialityId
		// });
	}
	
	update(request: UpdateSubcategoryRequest) {
		// return this.http.put(`${this.testApiUrl}/doctors`, {
		//   "Id": request.Id,
		//   "Name": request.Name,
		//   "Surname": request.Surname,
		//   "PhotoPath": request.PhotoPath,
		//   "SpecialityId": request.SpecialityId,
		// });
	}
	
	delete(id: number) {
		// return this.http.delete(`${this.testApiUrl}/doctors?id=${id}`)
	}
}
