import {Component, inject} from '@angular/core';
import {SubcategoryService} from '../../../../services/subcategory/subcategory.service';
import {SubcategoryResponse} from '../../../../models/subcategory/subcategory-response';

@Component({
  selector: 'app-admin-subcategories',
  imports: [],
  templateUrl: './admin-subcategories.component.html',
  standalone: true,
  styleUrl: './admin-subcategories.component.css'
})
export class AdminSubcategoriesComponent {
  private subcategoryService: SubcategoryService = inject(SubcategoryService);
  // protected subcategories: Record<number, SubcategoryResponse[]> = [];
  //
  //
  // constructor() {
  //   this.subcategoryService.get().subscribe(subcategories => {
  //     this.subcategories = this.groupSubcategoriesByCategory(subcategories);
  //   })
  // }
  //
  // private groupSubcategoriesByCategory(subcategories: SubcategoryResponse[]): Record<number, SubcategoryResponse[]> {
  //   return subcategories.reduce((acc, subcategory) => {
  //     if (!acc[subcategory.CategoryId]) {
  //       acc[subcategory.CategoryId] = [];
  //     }
  //     acc[subcategory.CategoryId].push(subcategory);
  //     return acc;
  //   }, {} as Record<number, SubcategoryResponse[]>);
  // }

  protected groupedSubcategories: { category: SubcategoryResponse; subcategories: SubcategoryResponse[] }[] = [];

  constructor() {
    let subcategoriesArray: SubcategoryResponse[] = [];

    this.subcategoryService.get().subscribe(categories => {
      subcategoriesArray = categories;
    })

    this.groupedSubcategories = this.groupSubcategoriesByCategory(subcategoriesArray);
  }

  private groupSubcategoriesByCategory(subcategories: SubcategoryResponse[]) {
    const grouped: { category: SubcategoryResponse; subcategories: SubcategoryResponse[] }[] = [];

    subcategories.forEach(subcategory => {
      let group = grouped.find(g => g.category.CategoryId === subcategory.CategoryId);
      if (!group) {
        group = {category: subcategory, subcategories: []};
        grouped.push(group);
      }
      group.subcategories.push(subcategory);
    });

    return grouped;
  }

}
