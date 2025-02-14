import { Request } from './request.model'
import { UserCar } from '../../home-page/models/userCar.model'

export interface CarRequest extends Request {
    userCar: UserCar
}