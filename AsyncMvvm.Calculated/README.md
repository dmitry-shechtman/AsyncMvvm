AsyncMVVM Calculated Properties integration
===========================================

AsyncMVVM library Calculated Properties integration.

Usage
-----

### NuGet Package ###

    Install-Package Ditto.AsyncMvvm.Calculated

### Binding ###

1. Derive your view model from `CalculatedAsyncBindableBase`:

        using Ditto.AsyncMvvm.Caclulated;

        class UniversalAnswerViewModel : CalculatedAsyncBindableBase
        {
        }

2. Implement the asynchronous method for calculating the value of the property:

        private async Task<int?> GetAnswerAsync()
        {
            await Task.Delay(TimeSpan.FromDays(7500000 * 365.25));
            return 42;
        }

3. Define the property:

        public int? Answer
        {
            get { return Property.Get(GetAnswerAsync); }
        }

### Validation ###

1. Determine the type of the validation error (e.g. `string`).

2. Derive your view model from the corresponding `CalculatedAsyncValidatableBindableBase`:

        using Ditto.AsyncMvvm.Caclulated;

        class UniversalAnswerViewModel : CalculatedAsyncValidatableBindableBase<string>
        {
            private int _answer;
            public int Answer
            {
                get { return _answer; }
                set { SetProperty(ref _answer, value); }
            }
        }

3. Override `GetErrorsAsync()`:

        protected override async Task<IEnumerable<string>> GetErrorsAsync(string propertyName)
        {
            if (propertyName == "Answer")
            {
                await Task.Delay(TimeSpan.FromDays(7500000 * 365.25));
                if (Answer != 42)
                    yield return "Answer is wrong.";
            }
        }

Notice
------

   Copyright © Dmitry Shechtman 2014-2015

   Licensed under the Apache License, Version 2.0 (the "License").

   You may obtain a copy of the License at
   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.

Links
-----

* [Code](https://github.com/dmitry-shechtman/AsyncMvvm)
* [Wiki](https://github.com/dmitry-shechtman/AsyncMvvm/wiki)
* [Blog](https://shecht.wordpress.com/category/asyncmvvm/)
* [NuGet](https://nuget.org/packages/Ditto.AsyncMvvm.Calculated)
