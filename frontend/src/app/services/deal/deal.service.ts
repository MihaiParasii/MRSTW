import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {CategoryResponse} from '../../models/category/category-response';
import {Observable, Subscriber} from 'rxjs';
import {CreateCategoryRequest} from '../../models/category/create-category-request';
import {UpdateCategoryRequest} from '../../models/category/update-category-request';
import {DealResponse} from '../../models/deal/deal-response';

@Injectable({
  providedIn: 'root'
})
export class DealService {
  http = inject(HttpClient);

  baseApiUrl = "https://darom.md/"
  testApiUrl = "http://localhost:5079/api/v1"


  private deals: Array<DealResponse> = [
    {Id: 1, Name: "Deal 1", Description: "Description 1"},
    {Id: 2, Name: "Deal 2", Description: "Description 2"},
    {Id: 3, Name: "Deal 3", Description: "Description 3"},
    {Id: 4, Name: "Deal 4", Description: "Description 4"},
    {Id: 5, Name: "Deal 5", Description: "Description 5"},
    {Id: 6, Name: "Deal 6", Description: "Description 6"},
    {Id: 7, Name: "Deal 7", Description: "Description 7"},
    {Id: 8, Name: "Deal 8", Description: "Description 8"},
    {Id: 9, Name: "Deal 9", Description: "Description 9"},
  ]

  get(): Observable<Array<DealResponse>> {
    return Observable.create((observer: Subscriber<any>) => {
      observer.next(this.deals);
      observer.complete();
    });
    // return Observable<this.categories>;
    // return this.http.get<Doctor[]>(`${this.testApiUrl}/doctors`)
  }

  getTop9() : Observable<Array<DealResponse>> {
    return Observable.create((observer: Subscriber<any>) => {
      observer.next(this.deals);
      observer.complete();
    })
  }

  getById(id: number): Observable<DealResponse> {
    // return this.http.get<Doctor>(`${this.testApiUrl}/doctors/${id}`)

    return Observable.create((observer: Subscriber<any>) => {
      observer.next(this.deals[id - 1]);
      observer.complete();
    });
  }

  create(request: CreateCategoryRequest) {
    // return this.http.post(`${this.testApiUrl}/doctor`, {
    //   "Name": request.Name,
    //   "Surname": request.Surname,
    //   "PhotoPath": request.PhotoPath,
    //   "SpecialityId": request.SpecialityId
    // });
  }

  update(request: UpdateCategoryRequest) {
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
