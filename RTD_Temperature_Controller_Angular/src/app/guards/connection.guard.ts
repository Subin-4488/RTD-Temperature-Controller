import { CanActivateFn } from '@angular/router';

export const connectionGuard: CanActivateFn = (route, state) => {
  alert("Permission denied")
  return false;
};
