// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;

// public class Store_Pro2 : MonoBehaviour
// {
//     public GameObject storeUI;
//     public GameObject[] imagePrefabs; // 추가할 이미지의 프리팹

//     public Transform[] contents; // ScrollView의 Content 배열

//     public GameObject selectedImagePrefab;
//     public GameObject selectedUIPrefab;

//     [Header("재료들 샀을 때 뜨는 UI")]
//     // public GameObject buyStrawberry_UI;
//     // public GameObject buyApple_UI;
//     // public GameObject buyPork_UI;
//     // public GameObject buyFish_UI;
//     // public GameObject buySoysource_UI;
//     // public GameObject buyOriginalMak_UI;
//     private GameObject lastClickedImage; // 마지막에 클릭한 이미지 저장
//     public Dictionary<string, TextMeshProUGUI> imageTextMapping = new Dictionary<string, TextMeshProUGUI>();
//     private TextMeshProUGUI previousActiveText;

//     public TextMeshProUGUI buyFirst_txt;
//     public TextMeshProUGUI buyStrawberry_txt;
//     public TextMeshProUGUI buyApple_txt;
//     public TextMeshProUGUI buyPork_txt;
//     public TextMeshProUGUI buyFish_txt;
//     public TextMeshProUGUI buySoysource_txt;
//     public TextMeshProUGUI buyOriginalMak_txt;
//     public Player player;

//     void Start()
//     {
//         player = player.GetComponent<Player>();
//         // 딕셔너리에 이미지 이름과 TextMeshProUGUI를 매핑합니다.
//         imageTextMapping.Add("ItemImage_Strawberry", buyStrawberry_txt);
//         imageTextMapping.Add("ItemImage_Apple", buyApple_txt);
//         imageTextMapping.Add("ItemImage_Pork", buyPork_txt);
//         imageTextMapping.Add("ItemImage_Fish", buyFish_txt);
//         imageTextMapping.Add("ItemImage_Source", buySoysource_txt);
//         imageTextMapping.Add("ItemImage_Mak", buyOriginalMak_txt);

//         // 추가적인 이미지와 TextMeshProUGUI 매핑

//         // 초기화 코드
//         foreach (var text in imageTextMapping.Values)
//         {
//             text.text = "골라~ 골라~ 싸다 싸~      날마다 오는 기회가 아니여~";
//         }

//         // 첫 번째 텍스트를 활성화하고 내용을 변경
//         buyFirst_txt.gameObject.SetActive(true);
//         buyFirst_txt.text = "골라~ 골라~ 싸다 싸~      날마다 오는 기회가 아니여~";
//     }

//     private bool isImageAdded = false; // 이미지가 추가되었는지 여부를 나타내는 플래그
//     //private bool isUIAdded = false;
//     public Recipe_Pro recipePro;

//     public List<GameObject> saveRecipe; // 상점에서 이미지를 클릭하면 saveRecipe 리스트에 순차점으로 저장이 된다

//     // 해당 프리팹을 몇 번 선택했는지 확인
//     private Dictionary<GameObject, int> imagePrefabCount = new Dictionary<GameObject, int>();

//     public void SetSelectedImagePrefab(GameObject selectedPrefab)
//     {
//         selectedImagePrefab = selectedPrefab;
//         isImageAdded = false; // 새로운 이미지를 선택하면 플래그 초기화

//         // 이미지 프리팹의 카운트를 확인하고 없으면 0으로 초기화
//         if (!imagePrefabCount.ContainsKey(selectedPrefab))
//         {
//             imagePrefabCount[selectedPrefab] = 0;
//         }
//     }
//     public void SetSelectedUI(GameObject selectedUI) // 선택한 UI 저장
//     {
//         selectedUIPrefab = selectedUI;
//         SaveRecipeImage(); // 선택한 UI 리스트에 추가
//         //isUIAdded = false; // 새로운 UI를 선택하면 플래그 초기화 // 추후 필요가능성 O
//     }

//     public void AddImage()
//     {
//         if (selectedImagePrefab != null && !isImageAdded)
//         {
//             for (int i = 0; i < contents.Length; i++)
//             {
//                 Transform content = contents[i];
//                 if (!ContainsImage(content) && imagePrefabCount[selectedImagePrefab] == 0)
//                 {
//                     imagePrefabCount[selectedImagePrefab]++; // 이미지 프리팹의 카운트 증가

//                     GameObject newImage = Instantiate(selectedImagePrefab, content);

//                     // Content의 크기 조정
//                     RectTransform contentRectTransform = contents[i].GetComponent<RectTransform>();
//                     contentRectTransform.sizeDelta += new Vector2(0, newImage.GetComponent<RectTransform>().sizeDelta.y);
//                     //isUIAdded = true;

//                     lastClickedImage = newImage; // 마지막에 클릭한 이미지 업데이트

//                     //isImageAdded = true; // 이미지 추가 true
//                     break;
//                 }
//             }
//         }
//         OnImageAdded(); // 이미지 추가가 완료되었음을 알림

//     }

//     public void OnImageAdded() // 이미지 추가가 완료되면 호출하는 메서드
//     {
//         // 이미지 추가 여부를 다시 초기화
//         isImageAdded = false;

//         // Debug.Log를 이미지 추가가 완료된 후에 호출
//         Debug.Log($"Prefab {selectedImagePrefab.name} Count: {imagePrefabCount[selectedImagePrefab]}");

//         if (imageTextMapping.TryGetValue(selectedImagePrefab.name, out TextMeshProUGUI correspondingText))
//         {
//             // 이전에 활성화된 텍스트 비활성화
//             if (previousActiveText != null)
//             {
//                 previousActiveText.gameObject.SetActive(false);
//             }

//             buyFirst_txt.gameObject.SetActive(false);

//             // 새로운 텍스트 활성화
//             correspondingText.gameObject.SetActive(true);
//             previousActiveText = correspondingText;

//             switch (selectedImagePrefab.name)
//             {
//                 case "ItemImage_Strawberry":
//                     correspondingText.text = "딸기라. . 달달하니 맛있겠군!";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//                 case "ItemImage_Apple":
//                     correspondingText.text = "사과같은 내 얼굴~ 허허";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//                 case "ItemImage_Pork":
//                     correspondingText.text = "고기는 항상 맛있다구!         든든하게 챙기시게!";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//                 case "ItemImage_Fish":
//                     correspondingText.text = "이 놈 아주 팔팔하구먼!        아주 싱싱해!";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//                 case "ItemImage_Source":
//                     correspondingText.text = "모든 맛있는 요리에는           항상 간장이 들어간다구~";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//                 case "ItemImage_Mak":
//                     correspondingText.text = "다양한 막걸리를 만드려면  기본 막걸리부터 챙기시게!";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//                 // 다른 이미지에 대한 경우도 추가할 수 있습니다.
//                 default:
//                     correspondingText.text = "싸다싸~골라~";
//                     buyFirst_txt.gameObject.SetActive(false);
//                     break;
//             }
//         }
//     }

//     public void SaveRecipeImage()
//     {
//         if (saveRecipe.Count < 6) // 5개까지만 추가
//         {
//             // 이미 저장된 UI 프리팹이 아니라면 추가해라
//             if (!saveRecipe.Contains(selectedUIPrefab))
//             {
//                 saveRecipe.Add(selectedUIPrefab);
//             }
//         }
//     }

//     public bool ContainsImage(Transform content)
//     {
//         return content.childCount > 0; // content에 이미지프리팹 있는지 확인
//     }

//     public void CloseStoreUI()
//     {
//         storeUI.SetActive(false);
//         if (previousActiveText != null)
//         {
//             previousActiveText.gameObject.SetActive(false);
//         }

//         // if (lastClickedImage != null)
//         // {
//         //     lastClickedImage.SetActive(false);
//         //     lastClickedImage = null;
//         // }

//         buyFirst_txt.gameObject.SetActive(true);
//         player.isMenuPaperOpen = false;
//     }

//     public void OpenStoreUI()
//     {
//         storeUI.SetActive(true);

//         if (lastClickedImage == null)
//         {
//             buyFirst_txt.gameObject.SetActive(true);
//         }
//     }

// }