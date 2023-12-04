import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

/**
 * Custom guard to check if the user is connected before allowing access to a route.
 * @param route The activated route snapshot.
 * @param state The router state snapshot.
 * @returns A boolean indicating whether the route can be activated.
 */

export const connectionGuard: CanActivateFn = (route, state) => {
  
  const router: Router = inject(Router);
  if (localStorage.getItem("connected") == "true")
    return true;
  
  alert("Permission denied")
  router.navigateByUrl("/")
  return false;
};