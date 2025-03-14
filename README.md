# MergeBrawl (ID: 416094 на Яндекс Играх)

Игра жанра merge. Прошла проверку и опубликована на площадке Яндекс.Игры (https://yandex.ru/games/app/416094?lang=ru)

Реализована на фреймворке HECS (гибридная ECS). В основе глобальная стейт система управляющая переходами между состояниями игры. Разрабатывалась в течении 2-х недель командой из двух человек. В данном проекте я по большей части занимался внутренней логикой игры, практически не касаясь работы с UI. Для аналитики использовалась GameAnalytics.

- [Yandex](Features/YandexSystem) - системы, jslib и компоненты связанные с работой SDK Yndex, все полностью самописное.
- [GameAnalytics](Systems/GameAnalytics/GameAnalyticsSystem.cs) - система гейм аналитики.
- [NumberConverter](Systems/UI/ConvertNumbersSystem.cs) - небольшая система для приведения больших чисел к виду 10k, 10,4k и тд, с возможностью указывать длинну строки и выводом только значимых символов.
- [MergeSystem](Systems/GameLogic/MergeSystem.cs) - система объединения объектов.
- [EndGameZoneSystem](Systems/GameLogic/EndGameZoneSystem.cs) - система для определения завершения игры, срабатывает только в случае, если объект находясь в зоне-триггере имеет контакт с каким-либо другим объектом, с которым он не может быть объединен.
- [SaveSystem](Systems/SavePlayer/SavePlayerSystem.cs) - система сохранения данных игрока.
- [ObjectSystems](Systems/SceneObjects) - системы игровых объектов (детект возможности объединения, абилки уничтожения, апгрейда, перемешивания).
- [SoundSystems](Systems/Sounds/FXSoundSystem.cs) - системы для управления звуками в игре. 

![изображение](https://github.com/user-attachments/assets/e12bdcfc-74c8-4478-95af-c9ed2aae3206)
