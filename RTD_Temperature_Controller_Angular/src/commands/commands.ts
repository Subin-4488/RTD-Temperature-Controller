export const DEVICE_COMMANDS = {
  //Manual mode commands
  SET_AUTOMATIC_MODE: 'SET MOD ATM\r',
  //For red LED
  SET_RLED_ON: 'SET LED ON R\r',
  SET_RLED_OFF: 'SET LED OFF R\r',
  //For green LED
  SET_GLED_ON: 'SET LED ON G\r',
  SET_GLED_OFF: 'SET LED OFF G\r',
  //For blue LED
  SET_BLED_ON: 'SET LED ON B\r',
  SET_BLED_OFF: 'SET LED OFF B\r',
  //For PWM
  SET_PWM: 'SET DTY ',
  //For temperature
  GET_TEMPERATURE: 'GET TMPM\r',
  //For resistance
  GET_RESISTANCE: 'GET RES\r',
  //For EEPROM
  GET_EEPROM: 'GET EPR\r',
  //For Start button
  GET_START_BUTTON: 'GET BTN START\r',
  //For Stop button
  GET_STOP_BUTTON: 'GET BTN STOP\r',
};
