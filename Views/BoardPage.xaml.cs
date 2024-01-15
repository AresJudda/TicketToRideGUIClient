using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using TicketToRideGUI.GameBoardService;
using TicketToRideGUI.Logic;

namespace TicketToRideGUI.Views
{
    public partial class BoardPage : Page, IBoardGameServiceCallback
    {
        private UserReference _userReference;
        private LobbieReference _lobbieReference;
        private bool clikTrainCards = false;
        private bool clikDeckRouteCards = false;
        private bool clickRouteCards = false;
        private bool clickClaimRoute = false;
        private int countCards = 0;
        private List<Button> activeButtons = new List<Button>();



        List<string> deckDestinationCards = new List<string>()
        {
            "/Images/Calgary-Phoenix.png",
            "/Images/Calgary-SaltLake.png",
            "/Images/Denver-El Paso.png",
            "/Images/Denver-Pittsburch.png",
            "/Images/Doluth-ElPaso.png",
            "/Images/Doluth-Houston.png",
            "/Images/KansasCity-Houston.png",
            "/Images/LosAngeles-Chicago.png",
            "/Images/LosAngeles-NewYork.png",
            "/Images/Montreal-Atlanta.png",
            "/Images/NewYork-Atlanta.png",
            "/Images/SanFrancisco-Atlanta.png",
            "/Images/Seattle-NewYork.png",
            "/Images/Vancouver-Montreal.png",
            "/Images/Winnipec-Houston.png"
        };

        List<string> deckTrainCards = new List<string>()
        {
            "/Images/trainRed.png",
            "/Images/trainPink.png",
            "/Images/trainYellow.png",
            "/Images/trainOrange.png",
            "/Images/trainBlack.png",
            "/Images/trainGreen.png",
            "/Images/trainBlue.png",
            "/Images/trainWhite.png",
            "/Images/locomotive.png"
        };

        private Dictionary<string, (City Origin, City Destinity)> imageToDestinationCityMap = new Dictionary<string, (City, City)>
        {
            { "Calgary-Phoenix.png", (City.Calgary, City.Phoenix) },
            { "Calgary-SaltLake.png", (City.Calgary, City.SaltLakeCity) },
            { "Denver-El Paso.png", (City.Denver, City.ElPaso) },
            { "Denver-Pittsburch.png", (City.Denver, City.Pittsburgh) },
            { "Doluth-ElPaso.png", (City.Duluth, City.ElPaso) },
            { "Doluth-Houston.png", (City.Duluth, City.Houston) },
            { "KansasCity-Houston.png", (City.KansasCity, City.Houston) },
            { "LosAngeles-Chicago.png", (City.LosAngeles, City.Chicago) },
            { "LosAngeles-NewYork.png", (City.LosAngeles, City.NewYork)  },
            { "Montreal-Atlanta.png", (City.Montreal, City.Atlanta) },
            { "NewYork-Atlanta.png", (City.NewYork, City.Atlanta) },
            { "SanFrancisco-Atlanta.png", (City.SanFrancisco, City.Atlanta) },
            { "Seattle-NewYork.png", (City.Seattle, City.NewYork) },
            { "Vancouver-Montreal.png", (City.Vancouver, City.Montreal) },
            { "Winnipec-Houston.png", (City.Winnipeg, City.Houston) }
        };

        private Dictionary<string, TrainColor> imageToTrainColorMap = new Dictionary<string, TrainColor>
        {
            { "locomotive.png", TrainColor.Locomotive },
            { "trainBlack.png", TrainColor.Black },
            { "trainBlue.png", TrainColor.Blue },
            { "trainGreen.png", TrainColor.Green },
            { "trainOrange.png", TrainColor.Orange },
            { "trainPink.png", TrainColor.Pink },
            { "trainRed.png", TrainColor.Red },
            { "trainWhite.png", TrainColor.White },
            { "trainYellow.png", TrainColor.Yellow }
        };

        private Dictionary<string, List<Button>> routeToButtonsMap;

        public BoardPage()
        {
            InitializeComponent();
            InitializeRoutes();
            _lobbieReference = LobbieReference.GetInstance();
            _userReference = UserReference.GetInstance();
            RegisterCallback();
            StartGame();
        }

        private void InitializeRoutes()
        {
            routeToButtonsMap = new Dictionary<string, List<Button>>
            {
                { "Vancouver-Calgary", new List<Button> { btnBrownVancouverCalgary01, btnBrownVancouverCalgary02, btnBrownVancouverCalgary03 } },
                { "Calgary-Winnipeg", new List<Button> { btnWhiteCalgaryWinipec01, btnWhiteCalgaryWinipec02, btnWhiteCalgaryWinipec03, btnWhiteCalgaryWinipec04, btnWhiteCalgaryWinipec05, btnWhiteCalgaryWinipec06 } },
                { "Winnipeg-SaultSaintMarie", new List<Button> { btnBrownWinipecSaultSTMarie01, btnBrownWinipecSaultSTMarie02, btnBrownWinipecSaultSTMarie03, btnBrownWinipecSaultSTMarie04, btnBrownWinipecSaultSTMarie05, btnBrownWinipecSaultSTMarie06 } },
                { "SaultSaintMarie-Montreal", new List<Button> { btnBlackMontrealSaultStMarie01, btnBlackMontrealSaultStMarie02, btnBlackMontrealSaultStMarie03, btnBlackMontrealSaultStMarie04, btnBlackMontrealSaultStMarie05 } },
                { "Montreal-Boston", new List<Button> { btnBrownMontrealBoston01, btnBrownMontrealBoston03 } },
                { "Montreal-Boston1", new List<Button> { btnBrownMontrealBoston02, btnBrownMontrealBoston04 } },
                { "Vancouver-Seattle", new List<Button> { btnBrownSeatleVancouver01 } },
                { "Vancouver-Seattle1", new List<Button> { btnBrownSeatleVancouver02 } },
                { "Seattle-Calgary", new List<Button> { btnBrownSeatleCalgary01, btnBrownSeatleCalgary02, btnBrownSeatleCalgary03, btnBrownSeatleCalgary04 } },
                { "Calgary-Helena", new List<Button> { btnBrownCalgaryHelena01, btnBrownCalgaryHelena02, btnBrownCalgaryHelena03, btnBrownCalgaryHelena04 } },
                { "Helena-Winnipeg", new List<Button> { btnBlueHelenaWinnipec01, btnBlueHelenaWinnipec02, btnBlueHelenaWinnipec03, btnBlueHelenaWinnipec04 } },
                { "Winnipeg-Duluth", new List<Button> { btnBlackWinnipecDuluth01, btnBlackWinnipecDuluth02, btnBlackWinnipecDuluth03, btnBlackWinnipecDuluth04 } },
                { "Duluth-SaultSaintMarie", new List<Button> { btnBrownDuluthSaultSTMarie01, btnBrownDuluthSaultSTMarie02, btnBrownDuluthSaultSTMarie03 } },
                { "SaultSaintMarie-Toronto", new List<Button> { btnBrownSaultSTMarieToronto01, btnBrownSaultSTMarieToronto02 } },
                { "Toronto-Montreal", new List<Button> { btnBrownTorontoMontreal01, btnBrownTorontoMontreal02, btnBrownTorontoMontreal03 } },
                { "Portland-Seattle", new List<Button> { btnBrownPortlandSeatle01 } },
                { "Portland-Seattle1", new List<Button> { btnBrownPortlandSeatle02 } },
                { "Seattle-Helena", new List<Button> {btnYellowSeattleHelena01, btnYellowSeattleHelena02, btnYellowSeattleHelena03, btnYellowSeattleHelena04, btnYellowSeattleHelena05, btnYellowSeattleHelena06 } },
                { "Helena-Duluth", new List<Button> {btnOrangeHelenaDuluth01, btnOrangeHelenaDuluth02, btnOrangeHelenaDuluth03, btnOrangeHelenaDuluth04, btnOrangeHelenaDuluth05, btnOrangeHelenaDuluth06 } },
                { "Duluth-Toronto", new List<Button> { btnRoseDuluthToronto01, btnRoseDuluthToronto02, btnRoseDuluthToronto03, btnRoseDuluthToronto04, btnRoseDuluthToronto05, btnRoseDuluthToronto06 } },
                { "Toronto-Pittsburgh", new List<Button> { btnBrownTorontoPittsburch01, btnBrownTorontoPittsburch02 } },
                { "Montreal-NewYork", new List<Button> { btnBlueNewYorkMontreal01, btnBlueNewYorkMontreal02, btnBlueNewYorkMontreal03 } },
                { "NewYork-Boston", new List<Button> { btnYellowNewYorkBoston01, btnYellowNewYorkBoston02 } },
                { "NewYork-Boston1", new List<Button> { btnRedNewYorkBoston01, btnRedNewYorkBoston02 } },
                { "Portland-SanFrancisco", new List<Button> { btnGreenSanFranciscoPortland01, btnGreenSanFranciscoPortland02, btnGreenSanFranciscoPortland03, btnGreenSanFranciscoPortland04, btnGreenSanFranciscoPortland05 } },
                { "Portland-SanFrancisco1", new List<Button> { btnRoseSanFranciscoPortland01, btnRoseSanFranciscoPortland02, btnRoseSanFranciscoPortland03, btnRoseSanFranciscoPortland04, btnRoseSanFranciscoPortland05 } },
                { "Portland-SaltLakeCity", new List<Button> { btnBluePortlandSaltLakeCity01, btnBluePortlandSaltLakeCity02, btnBluePortlandSaltLakeCity03, btnBluePortlandSaltLakeCity04, btnBluePortlandSaltLakeCity05 } },
                { "SanFrancisco-SaltLakeCity", new List<Button> { btnOrangeSanFranciscoSaltLakeCity01, btnOrangeSanFranciscoSaltLakeCity02, btnOrangeSanFranciscoSaltLakeCity03, btnOrangeSanFranciscoSaltLakeCity04, btnOrangeSanFranciscoSaltLakeCity05 } },
                { "SanFrancisco-SaltLakeCity1", new List<Button> { btnWhiteSanFranciscoSaltLakeCity01, btnWhiteSanFranciscoSaltLakeCity02, btnWhiteSanFranciscoSaltLakeCity03, btnWhiteSanFranciscoSaltLakeCity04, btnWhiteSanFranciscoSaltLakeCity05 } },
                { "SaltLakeCity-Helena", new List<Button> { btnRoseSaltLakeCityHelena01, btnRoseSaltLakeCityHelena02, btnRoseSaltLakeCityHelena03 } },
                { "Helena-Denver", new List<Button> { btnGreenDenverHelena01, btnGreenDenverHelena02, btnGreenDenverHelena03, btnGreenDenverHelena04 } },
                { "SaltLakeCity-Denver", new List<Button> { btnRedSaltLakeCityDenver01, btnRedSaltLakeCityDenver02, btnRedSaltLakeCityDenver03 } },
                { "SaltLakeCity-Denver1", new List<Button> { btnYellowSaltLakeCityDenver01, btnYellowSaltLakeCityDenver02, btnYellowSaltLakeCityDenver03 } },
                { "Helena-Omaha", new List<Button> { btnRedHelenaOmaha01, btnRedHelenaOmaha02, btnRedHelenaOmaha03, btnRedHelenaOmaha04, btnRedHelenaOmaha05 } },
                { "Omaha-Duluth", new List<Button> { btnBrownOmahaDuluth01, btnBrownOmahaDuluth03 } },
                { "Omaha-Duluth1", new List<Button> { btnBrownOmahaDuluth02, btnBrownOmahaDuluth04 } },
                { "Duluth-Chicago", new List<Button> { btnRedDuluthChicago01, btnRedDuluthChicago02, btnRedDuluthChicago03 } },
                { "Omaha-Chicago", new List<Button> { btnBlueOmahaChicago01, btnBlueOmahaChicago02, btnBlueOmahaChicago03, btnBlueOmahaChicago04 } },
                { "Chicago-Toronto", new List<Button> { btnWhiteChicagoToronto01, btnWhiteChicagoToronto02, btnWhiteChicagoToronto03, btnWhiteChicagoToronto04 } },
                { "Chicago-Pittsburgh", new List<Button> { btnOrangeChicagoPittsburch01, btnOrangeChicagoPittsburch02, btnOrangeChicagoPittsburch03 } },
                { "Chicago-Pittsburgh1", new List<Button> { btnBlackChicagoPittsburch01, btnBlackChicagoPittsburch02, btnBlackChicagoPittsburch03 } },
                { "Pittsburgh-NewYork", new List<Button> { btnWhitePittsburchNewYork01, btnWhitePittsburchNewYork02 } },
                { "Pittsburgh-NewYork1", new List<Button> { btnGreenNewYorkPittsburch01, btnGreenNewYorkPittsburch02 } },
                { "NewYork-Washington", new List<Button> { btnOrangeNewYorkWashington01, btnOrangeNewYorkWashington02 } },
                { "NewYork-Washington1", new List<Button> { btnBlackWashingtonNewYork01, btnBlackWashingtonNewYork02 } },
                { "SanFrancisco-LosAngeles", new List<Button> { btnYellowSanFranciscoLosAngeles01, btnYellowSanFranciscoLosAngeles02, btnYellowSanFranciscoLosAngeles03 } },
                { "SanFrancisco-LosAngeles1", new List<Button> { btnRoseSanFranciscoLosAngeles01, btnRoseSanFranciscoLosAngeles02, btnRoseSanFranciscoLosAngeles03 } },
                { "LosAngeles-LasVegas", new List<Button> { btnBrownLosAngelesLasVegas01, btnBrownLosAngelesLasVegas02 } },
                { "LasVegas-SaltLakeCity", new List<Button> { btnOrangeLasVegasSaltLakeCity01, btnOrangeLasVegasSaltLakeCity02, btnOrangeLasVegasSaltLakeCity03 } },
                { "LosAngeles-Phoenix", new List<Button> { btnBrownPhoenixLosAngeles01, btnBrownPhoenixLosAngeles02, btnBrownPhoenixLosAngeles03 } },
                { "LosAngeles-ElPaso", new List<Button> { btnBlackLosAngelesElPaso01, btnBlackLosAngelesElPaso02, btnBlackLosAngelesElPaso03, btnBlackLosAngelesElPaso04, btnBlackLosAngelesElPaso05, btnBlackLosAngelesElPaso06 } },
                { "Phoenix-Denver", new List<Button> { btnWhitePhoenixDenver01, btnWhitePhoenixDenver02, btnWhitePhoenixDenver03, btnWhitePhoenixDenver04, btnWhitePhoenixDenver05 } },
                { "Phoenix-SantaFe", new List<Button> { btnBrownSantaFePhoenix01, btnBrownSantaFePhoenix02, btnBrownSantaFePhoenix03 } },
                { "Phoenix-ElPaso", new List<Button> { btnBrownElPasoPhoenix01, btnBrownElPasoPhoenix02, btnBrownElPasoPhoenix03 } },
                { "ElPaso-SantaFe", new List<Button> { btnBrownSantaFeElPaso01, btnBrownSantaFeElPaso02 } },
                { "SantaFe-Denver", new List<Button> { btnBrownSantaFeDenver01, btnBrownSantaFeDenver02 } },
                { "Denver-KansasCity", new List<Button> { btnBlackKansasCityDenver01, btnBlackKansasCityDenver02, btnBlackKansasCityDenver03, btnBlackKansasCityDenver04 } },
                { "Denver-KansasCity1", new List<Button> { btnOrangeDenverKansasCity01, btnOrangeDenverKansasCity02, btnOrangeDenverKansasCity03, btnOrangeDenverKansasCity04 } },
                { "Omaha-KansasCity", new List<Button> { btnBrownKansasCityOmaha01 } },
                { "Omaha-KansasCity1", new List<Button> { btnBrownKansasCityOmaha02} },
                { "Denver-OklahomaCity", new List<Button> { btnRedDenverOklahomaCity01, btnRedDenverOklahomaCity02, btnRedDenverOklahomaCity03, btnRedDenverOklahomaCity04 } },
                { "SantaFe-OklahomaCity", new List<Button> { btnBlueSantaFeOklahomaCity01, btnBlueSantaFeOklahomaCity02 } },
                { "ElPaso-OklahomaCity", new List<Button> { btnYellowElPasoOklahomaCity01, btnYellowElPasoOklahomaCity02, btnYellowElPasoOklahomaCity03, btnYellowElPasoOklahomaCity04, btnYellowElPasoOklahomaCity05 } },
                { "ElPaso-Dallas", new List<Button> { btnRedElPasoDallas01, btnRedElPasoDallas02, btnRedElPasoDallas03, btnRedElPasoDallas04 } },
                { "ElPaso-Houston", new List<Button> { btnGreenElPasoHouston01, btnGreenElPasoHouston02, btnGreenElPasoHouston03, btnGreenElPasoHouston04, btnGreenElPasoHouston05, btnGreenElPasoHouston06 } },
                { "KansasCity-SaintLouis", new List<Button> { btnBlueKansasCitySaintLouis01, btnBlueKansasCitySaintLouis02 } },
                { "KansasCity-SaintLouis1", new List<Button> { btnRoseKansasCitySaintLouis01, btnRoseKansasCitySaintLouis02 } },
                { "SaintLouis-Chicago", new List<Button> { btnGreenSaintLouisChicago01, btnGreenSaintLouisChicago02 } },
                { "SaintLouis-Chicago1", new List<Button> { btnWhiteSaintLouisChicago01, btnWhiteSaintLouisChicago02 } },
                { "SaintLouis-Pittsburgh", new List<Button> { btnGreenSaintLouisPittsburch01, btnGreenSaintLouisPittsburch02, btnGreenSaintLouisPittsburch03, btnGreenSaintLouisPittsburch04, btnGreenSaintLouisPittsburch05 } },
                { "Pittsburgh-Washington", new List<Button> { btnBrownPittsburchWashington01, btnBrownPittsburchWashington02 } },
                { "SaintLouis-Nashville", new List<Button> { btnBrownSaintLouisNashville01, btnBrownSaintLouisNashville02 } },
                { "KansasCity-OklahomaCity", new List<Button> { btnBrownOklahomaCityKansasCity01, btnBrownOklahomaCityKansasCity03 } },
                { "KansasCity-OklahomaCity1", new List<Button> { btnBrownOklahomaCityKansasCity02, btnBrownOklahomaCityKansasCity04 } },
                { "OklahomaCity-Dallas", new List<Button> { btnBrownOklahomaCityDallas01, btnBrownOklahomaCityDallas03 } },
                { "OklahomaCity-Dallas1", new List<Button> { btnBrownOklahomaCityDallas02, btnBrownOklahomaCityDallas04 } },
                { "Dallas-Houston", new List<Button> { btnBrownDallasHouston01 } },
                { "Dallas-Houston1", new List<Button> { btnBrownDallasHouston02 } },
                { "OklahomaCity-LittleRock", new List<Button> { btnBrownLittleRockOklahomaCity01, btnBrownLittleRockOklahomaCity02 } },
                { "LittleRock-SaintLouis", new List<Button> { btnBrownSaintLouisLittleRock01, btnBrownSaintLouisLittleRock02 } },
                { "Nashville-Pittsburgh", new List<Button> { btnYellowNashvillePittsburch01, btnYellowNashvillePittsburch02, btnYellowNashvillePittsburch03, btnYellowNashvillePittsburch04 } },
                { "Pittsburgh-Raleigh", new List<Button> { btnBrownPittsburchRaleigh01, btnBrownPittsburchRaleigh02 } },
                { "Nashville-Raleigh", new List<Button> { btnBlackRaleighNashville01, btnBlackRaleighNashville03, btnBlackRaleighNashville03 } },
                { "Raleigh-Washington", new List<Button> { btnBrownWashingtonRaleigh01, btnBrownWashingtonRaleigh03 } },
                { "Raleigh-Washington1", new List<Button> { btnBrownWashingtonRaleigh02, btnBrownWashingtonRaleigh04 } },
                { "Raleigh-Charleston", new List<Button> { btnBrownRaleighCharleston01, btnBrownRaleighCharleston02 } },
                { "Dallas-LittleRock", new List<Button> { btnBrownLittleRockDallas01, btnBrownLittleRockDallas02 } },
                { "LittleRock-Nashville", new List<Button> { btnWhiteLittleRockNashville01, btnWhiteLittleRockNashville02, btnWhiteLittleRockNashville03 } },
                { "Nashville-Atlanta", new List<Button> { btnBrownAtlantaNashville01 } },
                { "Atlanta-Raleigh", new List<Button> { btnBrownRaleighAtlanta01, btnBrownRaleighAtlanta03 } },
                { "Atlanta-Raleigh1", new List<Button> { btnBrownRaleighAtlanta02, btnBrownRaleighAtlanta04 } },
                { "Atlanta-Charleston", new List<Button> { btnBrownCharlestonAtlanta01, btnBrownCharlestonAtlanta03 } },
                { "LittleRock-NewOrleans", new List<Button> { btnGreenLittleRockNewOrleans01, btnGreenLittleRockNewOrleans02, btnGreenLittleRockNewOrleans03 } },
                { "Houston-NewOrleans", new List<Button> { btnBrownHoustonNewOrleans01, btnBrownHoustonNewOrleans02 } },
                { "NewOrleans-Atlanta", new List<Button> { btnYellowNewOrleansAtlanta01, btnYellowNewOrleansAtlanta02, btnYellowNewOrleansAtlanta03, btnYellowNewOrleansAtlanta04 } },
                { "NewOrleans-Atlanta1", new List<Button> { btnOrangeNewOrleansAtlanta01, btnOrangeNewOrleansAtlanta02, btnOrangeNewOrleansAtlanta03, btnOrangeNewOrleansAtlanta04 } },
                { "NewOrleans-Miami", new List<Button> { btnRedNewOrleansMiami01, btnRedNewOrleansMiami02, btnRedNewOrleansMiami03, btnRedNewOrleansMiami04, btnRedNewOrleansMiami05, btnRedNewOrleansMiami06 } },
                { "Atlanta-Miami", new List<Button> { btnBlueAtlantaMiami01, btnBlueAtlantaMiami02, btnBlueAtlantaMiami03, btnBlueAtlantaMiami04, btnBlueAtlantaMiami05 } },
                { "Charleston-Miami", new List<Button> { btnRoseCharlestonMiami01, btnRoseCharlestonMiami02, btnRoseCharlestonMiami03, btnRoseCharlestonMiami04 } },
                { "Denver-Omaha", new List<Button> { btnRoseDenverOmaha01, btnRoseDenverOmaha02, btnRoseDenverOmaha03, btnRoseDenverOmaha04 } },
            };
        }




        private void InitializeMarkers(int numberOfPlayers)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                SetPlayerVisibility(RedPlayer, lblNameRedPlayer, txbNameRedPlayer, txbPlayerScoreRedPlayer, lblPlayerScoreRedPlayer, txbRedTrainGrouper, lblRedTrainGrouper, numberOfPlayers >= 1);
                SetPlayerVisibility(BluePlayer, lblNameBluePlayer, txbNameBluePlayer, txbPlayerScoreBluePlayer, lblPlayerScoreBluePlayer, txbBlueTrainGrouper, lblBlueTrainGrouper, numberOfPlayers >= 2);
                SetPlayerVisibility(YellowPlayer, lblNameYellowPlayer, txbNameYellowPlayer, txbPlayerScoreYellowPlayer, lblPlayerScoreYellowPlayer, txbYellowTrainGrouper, lblYellowTrainGrouper, numberOfPlayers >= 3);
                SetPlayerVisibility(GreenPlayer, lblNameGreenPlayer, txbNameGreenPlayer, txbPlayerScoreGreenPlayer, lblPlayerScoreGreenPlayer, txbGreenTrainGrouper, lblGreenTrainGrouper, numberOfPlayers >= 4);
                SetPlayerVisibility(BlackPlayer, lblNameBlackPlayer, txbNameBlackPlayer, txbPlayerScoreBlackPlayer, lblPlayerScoreBlackPlayer, txbBlackTrainGrouper, lblBlackTrainGrouper, numberOfPlayers >= 5);
            });
        }

        private void SetPlayerVisibility(UIElement player, TextBlock nameLabel, TextBox nameTextBox, TextBox scoreTextBox, TextBlock scoreLabel, TextBox trainTextBox, TextBlock trainLabel, bool isVisible)
        {
            player.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            nameLabel.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            nameTextBox.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            nameTextBox.IsReadOnly = true;
            scoreTextBox.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            scoreTextBox.IsReadOnly = true;
            scoreLabel.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            trainTextBox.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            trainTextBox.IsReadOnly = true;
            trainLabel.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ReportClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void claimRouteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                clikDeckRouteCards = false;
                clickRouteCards = false;
                clikTrainCards = false;
                Dispatcher.Invoke(() =>
                {
                    if (clickClaimRoute)
                    {
                        string idLobbie = _lobbieReference.GetKeylobbie();
                        Button clickedButton = (Button)sender;
                        string buttonName = clickedButton.Name;
                        string cityName = buttonName.Replace("btn", "");
                        InstanceContext context = new InstanceContext(this);
                        GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                        TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                        string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                        game.ClaimRoute(idLobbie, gamerTag, cityName);
                        NextTurn();
                    }
                });
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }

        }

        private void deckTrainCardClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                clikDeckRouteCards = false;
                clickRouteCards = false;
                clickClaimRoute = false;
                if (clikTrainCards)
                {
                    string idLobbie = _lobbieReference.GetKeylobbie();
                    InstanceContext context = new InstanceContext(this);
                    GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                    game.ClaimDeckTrainCard(idLobbie, gamerTag);
                    countCards++;
                    if (countCards >= 2)
                    {
                        NextTurn();
                    }
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }

        private void TrainCardClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                clikDeckRouteCards = false;
                clickRouteCards = false;
                clickClaimRoute = false;
                if (clikTrainCards)
                {
                    Image clickedImage = sender as Image;
                    if (clickedImage != null)
                    {
                        string imageName = Path.GetFileName(clickedImage.Source.ToString());
                        TrainColor color = GetTrainColorFromImageName(imageName);
                        TrainCard colorCard = new TrainCard();
                        colorCard.Color = color;
                        ClaimTrainCard(colorCard);
                        string idLobbie = _lobbieReference.GetKeylobbie();
                        InstanceContext context = new InstanceContext(this);
                        GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                        TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                        string gamerTag = player.GetGamerTag(_userReference.GetEmail());

                        if (color.Equals(TrainColor.Locomotive))
                        {
                            game.ClaimTrainCard(idLobbie, gamerTag, colorCard);
                            NextTurn();
                        }
                        else
                        {
                            countCards++;
                            game.ClaimTrainCard(idLobbie, gamerTag, colorCard);

                            if (countCards >= 2)
                            {
                                NextTurn();
                            }
                        }
                    }
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }

        }

        private TrainColor GetTrainColorFromImageName(string imageName)
        {
            string fileName = Path.GetFileName(imageName);
            return imageToTrainColorMap[fileName];
        }

        private void destinyCardClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (clickRouteCards)
                {
                    Image clickedImage = sender as Image;
                    if (clickedImage != null)
                    {
                        string imageName = Path.GetFileName(clickedImage.Source.ToString());
                        City[] cities = GetDestinationCityFromImageName(imageName);
                        string idLobbie = _lobbieReference.GetKeylobbie();
                        InstanceContext context = new InstanceContext(this);
                        GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                        TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                        string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                        game.StartRoute(idLobbie, gamerTag, cities);
                        NextTurn();
                    }
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }

        }

        private City[] GetDestinationCityFromImageName(string imageName)
        {
            string fileName = Path.GetFileName(imageName);
            var cities = imageToDestinationCityMap[fileName];
            City[] CitiesCard = new City[2];
            City origin = cities.Item1;
            City destination = cities.Item2;
            CitiesCard[0] = origin;
            CitiesCard[1] = destination;
            return CitiesCard;
        }

        private void deckDestinityCardClick(object sender, RoutedEventArgs e)
        {

        }

        private void StartGame()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                if (_lobbieReference.GetIsHost())
                {
                    List<string> playerNames = _lobbieReference.GetPlayers();
                    string[] playerNamesArray = playerNames.ToArray();
                    string IdLobbie = _lobbieReference.GetKeylobbie();
                    game.InicialiceGame(playerNamesArray, IdLobbie);
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }

        private void RegisterCallback()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                game.RegisterCallback(gamerTag);
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }


        private void SettingsClick(object sender, MouseButtonEventArgs e)
        {
            MenuPage menu = new MenuPage();
            this.NavigationService.Navigate(menu);
        }

        public void ReceiveResponseRegisterCallback(bool response)
        {
            if (!response)
            {
                MessageBox.Show(Properties.Resources.CallbackProblem);
            }

        }

        public void ReceiveResponseInicialiceGame(bool response)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (!response)
                    {
                        MessageBox.Show(Properties.Resources.InopportuneResponse);

                    }
                    else
                    {
                        string idLobbie = _lobbieReference.GetKeylobbie();
                        List<string> playerNames = _lobbieReference.GetPlayers();
                        string[] playerNamesArray = playerNames.ToArray();
                        InstanceContext context = new InstanceContext(this);
                        GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                        game.GetMarkers(idLobbie);
                        foreach (string gamer in playerNames)
                        {

                            game.GetCardsFromPlayer(gamer, idLobbie);
                            game.GetCardsFromBoard(idLobbie, playerNamesArray);
                        }
                        game.GetTurnPlayer(idLobbie);
                    }
                });
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }

        public void ReceiveResponseGetCardsFromPlayer(GamerBoard gamerBoard)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                InitializeCardsFromPlayer();
                int count = 0;
                foreach (var destinationCard in gamerBoard.DestinationCards)
                {
                    count++;
                    if (count == 1)
                    {
                        ShowDestinityCards(destinationCard, ImgDestinityCard);
                    }
                    else
                    {
                        ShowDestinityCards(destinationCard, ImgDestinityCard1);
                    }
                }
                foreach (var trainCard in gamerBoard.TrainCardsDeck)
                {
                    int valueColor = 0;
                    switch (trainCard.Color)
                    {
                        case (TrainColor.Red):

                            valueColor = 0;
                            break;
                        case (TrainColor.Pink):
                            valueColor = 1;
                            break;
                        case (TrainColor.Yellow):
                            valueColor = 2;

                            break;
                        case (TrainColor.Orange):
                            valueColor = 3;
                            break;
                        case (TrainColor.Black):
                            valueColor = 4;
                            break;
                        case (TrainColor.Green):
                            valueColor = 5;
                            break;
                        case (TrainColor.Blue):
                            valueColor = 6;
                            break;
                        case (TrainColor.White):
                            valueColor = 7;
                            break;
                        case (TrainColor.Locomotive):
                            valueColor = 8;
                            break;
                    }
                    CalculateNumberCards(valueColor);
                }
            });
        }

        public void ReceiveResponseGetCardsFromBoard(TrainCard[] boardCards)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                int countColor = 0;
                foreach (var trainCard in boardCards)
                {
                    countColor++;
                    int valueColor = 0;
                    switch (trainCard.Color)
                    {
                        case (TrainColor.Red):

                            valueColor = 0;
                            break;
                        case (TrainColor.Pink):
                            valueColor = 1;
                            break;
                        case (TrainColor.Yellow):
                            valueColor = 2;

                            break;
                        case (TrainColor.Orange):
                            valueColor = 3;
                            break;
                        case (TrainColor.Black):
                            valueColor = 4;
                            break;
                        case (TrainColor.Green):
                            valueColor = 5;
                            break;
                        case (TrainColor.Blue):
                            valueColor = 6;
                            break;
                        case (TrainColor.White):
                            valueColor = 7;
                            break;
                        case (TrainColor.Locomotive):
                            valueColor = 8;
                            break;
                    }
                    ShowTrainCards(valueColor, countColor);

                }
            });
        }

        private void InitializeCardsFromPlayer()
        {
            txtNumberBlackCards.Text = "0";
            txtNumberBlueCards.Text = "0";
            txtNumberGreenCards.Text = "0";
            txtNumberRedCards.Text = "0";
            txtNumberYellowCards.Text = "0";
            txtNumberLocomotiveCards.Text = "0";
            txtNumberOrangeCards.Text = "0";
            txtNumberWhiteCards.Text = "0";
            txtNumberPinkCards.Text = "0";

        }

        private void CalculateNumberCards(int valueColor)
        {
            switch (valueColor)
            {
                case 0:

                    if (int.TryParse(txtNumberRedCards.Text, out int numberRed))
                    {
                        numberRed++;
                        txtNumberRedCards.Text = numberRed.ToString();
                    }
                    else
                    {
                        txtNumberRedCards.Text = "1";
                    }
                    break;

                case 1:
                    if (int.TryParse(txtNumberPinkCards.Text, out int numberPink))
                    {
                        numberPink++;
                        txtNumberPinkCards.Text = numberPink.ToString();
                    }
                    else
                    {
                        txtNumberPinkCards.Text = "1";
                    }
                    break;
                case 2:
                    if (int.TryParse(txtNumberYellowCards.Text, out int numberYellow))
                    {
                        numberYellow++;
                        txtNumberYellowCards.Text = numberYellow.ToString();
                    }
                    else
                    {
                        txtNumberYellowCards.Text = "1";
                    }
                    break;
                case 3:
                    if (int.TryParse(txtNumberOrangeCards.Text, out int numberOrange))
                    {
                        numberOrange++;
                        txtNumberOrangeCards.Text = numberOrange.ToString();
                    }
                    else
                    {
                        txtNumberOrangeCards.Text = "1";
                    }
                    break;
                case 4:
                    if (int.TryParse(txtNumberBlackCards.Text, out int numberBlack))
                    {
                        numberBlack++;
                        txtNumberBlackCards.Text = numberBlack.ToString();
                    }
                    else
                    {
                        txtNumberBlackCards.Text = "1";
                    }
                    break;
                case 5:
                    if (int.TryParse(txtNumberGreenCards.Text, out int numberGreen))
                    {
                        numberGreen++;
                        txtNumberGreenCards.Text = numberGreen.ToString();
                    }
                    else
                    {
                        txtNumberGreenCards.Text = "1";
                    }
                    break;
                case 6:
                    if (int.TryParse(txtNumberBlueCards.Text, out int numberBlue))
                    {
                        numberBlue++;
                        txtNumberBlueCards.Text = numberBlue.ToString();
                    }
                    else
                    {
                        txtNumberBlueCards.Text = "1";
                    }
                    break;
                case 7:
                    if (int.TryParse(txtNumberWhiteCards.Text, out int numberWhite))
                    {
                        numberWhite++;
                        txtNumberWhiteCards.Text = numberWhite.ToString();
                    }
                    else
                    {
                        txtNumberWhiteCards.Text = "1";
                    }
                    break;
                case 8:
                    if (int.TryParse(txtNumberLocomotiveCards.Text, out int numberLocomotive))
                    {
                        numberLocomotive++;
                        txtNumberLocomotiveCards.Text = numberLocomotive.ToString();
                    }
                    else
                    {
                        txtNumberLocomotiveCards.Text = "1";
                    }
                    break;
            }
        }

        private void ShowTrainCards(int valueColor, int countColor)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                switch (countColor)
                {
                    case 1:
                        ImgTrainCard1.Source = new BitmapImage(new Uri(deckTrainCards[valueColor], UriKind.RelativeOrAbsolute));
                        ImgTrainCard1.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        ImgTrainCard2.Source = new BitmapImage(new Uri(deckTrainCards[valueColor], UriKind.RelativeOrAbsolute));
                        ImgTrainCard2.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        ImgTrainCard3.Source = new BitmapImage(new Uri(deckTrainCards[valueColor], UriKind.RelativeOrAbsolute));
                        ImgTrainCard3.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        ImgTrainCard4.Source = new BitmapImage(new Uri(deckTrainCards[valueColor], UriKind.RelativeOrAbsolute));
                        ImgTrainCard4.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        ImgTrainCard5.Source = new BitmapImage(new Uri(deckTrainCards[valueColor], UriKind.RelativeOrAbsolute));
                        ImgTrainCard5.Visibility = Visibility.Visible;
                        break;
                }
            });

        }

        private void ShowDestinityCards(DestinationCard destinationCard, Image imgDestinityCard)
        {
            int index = 0;

            switch (destinationCard.Origin)
            {
                case City.Calgary:
                    index = destinationCard.Destination == City.Phoenix ? 0 : 1;
                    break;
                case City.Denver:
                    index = destinationCard.Destination == City.ElPaso ? 2 : 3;
                    break;
                case City.Duluth:
                    index = destinationCard.Destination == City.ElPaso ? 4 : 5;
                    break;
                case City.KansasCity:
                    index = 6;
                    break;
                case City.LosAngeles:
                    index = destinationCard.Destination == City.Chicago ? 7 : 8;
                    break;
                case City.Montreal:
                    index = 9;
                    break;
                case City.NewYork:
                    index = 10;
                    break;
                case City.SanFrancisco:
                    index = 11;
                    break;
                case City.Seattle:
                    index = 12;
                    break;
                case City.Vancouver:
                    index = 13;
                    break;
                case City.Winnipeg:
                    index = 14;
                    break;
            }

            imgDestinityCard.Source = new BitmapImage(new Uri(deckDestinationCards[index], UriKind.RelativeOrAbsolute));
            imgDestinityCard.Visibility = Visibility.Visible;
        }

        public void ReceiveResponseGetTurnPlayer(GamerBoard gamerBoard)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    string gamerTagPlayer = gamerBoard.GamerTag;
                    lblTurno.Content = $"Es turno de: {gamerTagPlayer}";
                    TicketToRideService.IUserPlayer player = new TicketToRideService.UserPlayerClient();
                    string gamerTag = player.GetGamerTag(_userReference.GetEmail());
                    if (gamerTagPlayer == gamerTag)
                    {
                        CheckedClaimRoute(gamerBoard);
                    }
                });
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }

        }

        private void CheckedClaimRoute(GamerBoard gamerBoard)
        {
            try
            {
                string idLobbie = _lobbieReference.GetKeylobbie();
                InstanceContext context = new InstanceContext(this);
                GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                game.GetGamerInformation(idLobbie, gamerBoard.GamerTag);
                CheckCities(gamerBoard);
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }

        public void ReceiveResponseGetGamerInformation(GamerBoard gamerBoard)
        {
            if (!gamerBoard.SelectedRoute || gamerBoard.RouteFinished)
            {
                clickRouteCards = true;
            }
            else
            {
                clikTrainCards = true;
                clikDeckRouteCards = true;
                clickClaimRoute = true;

            }
        }

        private void CheckCities(GamerBoard gamerBoard)
        {
            try
            {
                if (gamerBoard.SelectedRoute)
                {
                    string idLobbie = _lobbieReference.GetKeylobbie();
                    InstanceContext context = new InstanceContext(this);
                    GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                    game.GetAvailableCyties(idLobbie, gamerBoard.GamerTag);
                }
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }

        }

        public void ReceiveResponseGetMarkers(Queue<GamerBoard> gamerBoards)
        {
            switch (gamerBoards.Count)
            {
                case 2:
                    InitializeMarkers(2);
                    break;
                case 3:
                    InitializeMarkers(3);
                    break;
                case 4:
                    InitializeMarkers(4);
                    break;
                case 5:
                    InitializeMarkers(5);
                    break;
            }
            SetDatailsFromMarkers(gamerBoards);
        }

        private void SetDatailsFromMarkers(Queue<GamerBoard> gamerBoards)
        {
            int count = 0;
            foreach (var player in gamerBoards)
            {
                count++;
                switch (count)
                {
                    case 1:
                        txbNameRedPlayer.Text = player.GamerTag.ToString();
                        txbPlayerScoreRedPlayer.Text = player.Points.ToString();
                        txbRedTrainGrouper.Text = player.GamerTrains.ToString();
                        break;
                    case 2:
                        txbNameBluePlayer.Text = player.GamerTag.ToString();
                        txbPlayerScoreBluePlayer.Text = player.Points.ToString();
                        txbBlueTrainGrouper.Text = player.GamerTrains.ToString();
                        break;
                    case 3:
                        txbNameYellowPlayer.Text = player.GamerTag.ToString();
                        txbPlayerScoreYellowPlayer.Text = player.Points.ToString();
                        txbYellowTrainGrouper.Text = player.GamerTrains.ToString();
                        break;
                    case 4:
                        txbNameGreenPlayer.Text = player.GamerTag.ToString();
                        txbPlayerScoreGreenPlayer.Text = player.Points.ToString();
                        txbGreenTrainGrouper.Text = player.GamerTrains.ToString();
                        break;
                    case 5:
                        txbNameBlackPlayer.Text = player.GamerTag.ToString();
                        txbPlayerScoreBlackPlayer.Text = player.Points.ToString();
                        txbBlackTrainGrouper.Text = player.GamerTrains.ToString();
                        break;
                }
            }
        }

        public void ReceiveUpdateCity(GamerBoard gamerBoard)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                string buttonName = $"btn{gamerBoard.CurrentLocation.ToString()}";
                Button destinationButton = FindName(buttonName) as Button;
                Color color = (Color)ColorConverter.ConvertFromString(gamerBoard.PlayerColor.ToString());
                SolidColorBrush brush = new SolidColorBrush(color);
                destinationButton.Background = brush;
            });
        }

        public void NextTurn()
        {
            try
            {
                clickRouteCards = false;
                clikTrainCards = false;
                clickClaimRoute = false;
                clikDeckRouteCards = false;
                countCards = 0;
                string idLobbie = _lobbieReference.GetKeylobbie();
                InstanceContext context = new InstanceContext(this);
                GameBoardService.IBoardGameService game = new GameBoardService.BoardGameServiceClient(context);
                game.GetTurnPlayer(idLobbie);
            }
            catch (TimeoutException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                MessageBox.Show(Properties.Resources.ServerNotAvailable);
            }
        }

        public void ClaimTrainCard(TrainCard cardTrainColor)
        {
            int valueColor = 0;
            switch (cardTrainColor.Color)
            {
                case (TrainColor.Red):

                    valueColor = 0;
                    break;
                case (TrainColor.Pink):
                    valueColor = 1;
                    break;
                case (TrainColor.Yellow):
                    valueColor = 2;

                    break;
                case (TrainColor.Orange):
                    valueColor = 3;
                    break;
                case (TrainColor.Black):
                    valueColor = 4;
                    break;
                case (TrainColor.Green):
                    valueColor = 5;
                    break;
                case (TrainColor.Blue):
                    valueColor = 6;
                    break;
                case (TrainColor.White):
                    valueColor = 7;
                    break;
                case (TrainColor.Locomotive):
                    valueColor = 8;
                    break;
            }
            CalculateNumberCards(valueColor);

        }

        public void ReceiveResponseClaimDeckTrainCard(TrainCard trainCard)
        {
            ClaimTrainCard(trainCard);
        }

        public void ReceiveResponseGetAvailableCyties(City[] cities, GamerBoard gamerBoard)
        {

            foreach (Button activeButton in activeButtons)
            {
                activeButton.BeginAnimation(SolidColorBrush.ColorProperty, null);
            }

            activeButtons.Clear();

            foreach (City city in cities)
            {
                string buttonName = $"btn{city.ToString()}";
                Button cityButton = FindName(buttonName) as Button;

                if (cityButton != null)
                {
                    activeButtons.Add(cityButton);
                    cityButton.BeginAnimation(SolidColorBrush.ColorProperty, null);

                    ColorAnimation colorAnimation = new ColorAnimation();
                    switch (gamerBoard.PlayerColor)
                    {
                        case PlayerColor.Yellow:
                            colorAnimation.To = (Color)ColorConverter.ConvertFromString("#FFAB00");
                            break;
                        case PlayerColor.Green:
                            colorAnimation.To = (Color)ColorConverter.ConvertFromString("#76FF03");
                            break;
                        case PlayerColor.Blue:
                            colorAnimation.To = (Color)ColorConverter.ConvertFromString("#0CB7F2");
                            break;
                        case PlayerColor.Red:
                            colorAnimation.To = (Color)ColorConverter.ConvertFromString("#E1567C");
                            break;
                        case PlayerColor.Black:
                            colorAnimation.To = (Color)ColorConverter.ConvertFromString("#212121");
                            break;
                    }

                    colorAnimation.Duration = new Duration(TimeSpan.FromSeconds(.8));
                    colorAnimation.AutoReverse = true;
                    colorAnimation.RepeatBehavior = RepeatBehavior.Forever;
                    SolidColorBrush newBrush = new SolidColorBrush();
                    cityButton.Background = newBrush;

                    newBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
                }
            }
        }

        public void ReceiveResponseClaimRoute(GamerBoard gamerBoard, string cityOrigin)
        {
            Dispatcher.Invoke(() =>
            {
                string currentCity = gamerBoard.CurrentLocation.ToString();
                string originDestinity = $"{cityOrigin}-{currentCity}";
                string destinityOrigin = $"{currentCity}-{cityOrigin}";
                if (routeToButtonsMap.ContainsKey(originDestinity))
                {
                    List<Button> buttons = routeToButtonsMap[originDestinity];
                    foreach (var button in buttons)
                    {
                        Color color = (Color)ColorConverter.ConvertFromString(gamerBoard.PlayerColor.ToString());
                        SolidColorBrush brush = new SolidColorBrush(color);
                        button.Background = brush;
                    }
                }
                else if (routeToButtonsMap.ContainsKey(destinityOrigin))
                {
                    List<Button> buttons = routeToButtonsMap[destinityOrigin];
                    foreach (var button in buttons)
                    {
                        Color color = (Color)ColorConverter.ConvertFromString(gamerBoard.PlayerColor.ToString());
                        SolidColorBrush brush = new SolidColorBrush(color);
                        button.Background = brush;
                    }
                }
            });
        }

        public void ReciveResponseFinalyGame(GamerBoard gamerBoard)
        {
            if (gamerBoard.IsWinner)
            {
                GameWonPage gameWon = new GameWonPage();
                this.NavigationService.Navigate(gameWon);
            }
            else
            {
                LostGame lostGame = new LostGame();
                this.NavigationService.Navigate(lostGame);
            }
        }

    }
}


