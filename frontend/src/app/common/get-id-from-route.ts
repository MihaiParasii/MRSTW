import { ActivatedRoute } from '@angular/router'

export function getIdFromRoute(route: ActivatedRoute, key: string = 'id', levelsUp: number = 0): number | null {
    let targetRoute = route
    
    for (let i = 0; i < levelsUp; i++) {
        if (!targetRoute.parent) {
            break
        }
        targetRoute = targetRoute.parent
    }
    
    return Number(targetRoute.snapshot.paramMap.get(key))
}
