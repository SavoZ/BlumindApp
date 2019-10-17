import { ActivatedRoute } from '@angular/router';

export class QueryParamsSearch {
    public static findParam(route: ActivatedRoute, paramName: string) {
        let result = null;

        route.params.subscribe(query => {
            result = query[paramName];
        });

        return result;
    }
}
