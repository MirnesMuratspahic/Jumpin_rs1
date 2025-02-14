import { Request } from './request.model'
import { UserFlat } from '../../home-page/models/userFlat.model'

export interface FlatRequest extends Request {
    userFlat: UserFlat
}